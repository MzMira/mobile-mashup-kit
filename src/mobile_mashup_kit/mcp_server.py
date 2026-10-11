"""MCP server for AI agents to interact with Mobile Mashup Kit."""
import asyncio
from pathlib import Path
from typing import Any

from .scanner import scan_apk, ScanResult
from .dumper import CodeDumper
from .hooker import RuntimeHooker
from .extractor import AssetExtractor


def create_server() -> Any:
    """Create and configure the MCP server."""
    try:
        from mcp.server import Server
        from mcp.server.stdio import stdio_server
        from mcp.types import Tool, TextContent
    except ImportError:
        raise RuntimeError(
            "mcp package not installed. Install with: pip install mcp"
        )

    server = Server("mobile-mashup-kit")

    @server.list_tools()
    async def list_tools() -> list[Tool]:
        """List available tools for AI agents."""
        return [
            Tool(
                name="scan_apk",
                description="Scan an APK file or decompiled directory to detect the game engine (Unity IL2CPP, Unity Mono, Cocos2d-x Lua, etc.)",
                inputSchema={
                    "type": "object",
                    "properties": {
                        "apk_path": {
                            "type": "string",
                            "description": "Path to the APK file or decompiled directory",
                        }
                    },
                    "required": ["apk_path"],
                },
            ),
            Tool(
                name="dump_code",
                description="Dump readable code from an APK using the appropriate tool (Il2CppDumper, jadx, unluac)",
                inputSchema={
                    "type": "object",
                    "properties": {
                        "apk_path": {
                            "type": "string",
                            "description": "Path to the APK file or decompiled directory",
                        },
                        "output_dir": {
                            "type": "string",
                            "description": "Output directory for dumped code",
                            "default": "./dumps",
                        },
                        "tool": {
                            "type": "string",
                            "enum": ["auto", "il2cppdumper", "il2cppinspector", "jadx", "unluac"],
                            "description": "Dumper tool to use",
                            "default": "auto",
                        },
                    },
                    "required": ["apk_path"],
                },
            ),
            Tool(
                name="hook_method",
                description="Hook a method in a running game using Frida",
                inputSchema={
                    "type": "object",
                    "properties": {
                        "package_name": {
                            "type": "string",
                            "description": "The game's package name (e.g., 'com.kakaogames.gdts')",
                        },
                        "class_name": {
                            "type": "string",
                            "description": "The class to hook",
                        },
                        "method_name": {
                            "type": "string",
                            "description": "The method to hook",
                        },
                        "spawn": {
                            "type": "boolean",
                            "description": "Whether to spawn the process or attach to it",
                            "default": True,
                        },
                    },
                    "required": ["package_name", "class_name", "method_name"],
                },
            ),
            Tool(
                name="list_processes",
                description="List running game processes on the connected device",
                inputSchema={
                    "type": "object",
                    "properties": {},
                },
            ),
            Tool(
                name="extract_assets",
                description="Extract game assets (textures, audio, meshes) from an APK",
                inputSchema={
                    "type": "object",
                    "properties": {
                        "apk_path": {
                            "type": "string",
                            "description": "Path to the APK file or decompiled directory",
                        },
                        "output_dir": {
                            "type": "string",
                            "description": "Output directory for extracted assets",
                            "default": "./assets",
                        },
                    },
                    "required": ["apk_path"],
                },
            ),
        ]

    @server.call_tool()
    async def call_tool(name: str, arguments: dict[str, Any]) -> list[TextContent]:
        """Execute a tool call from an AI agent."""
        if name == "scan_apk":
            result = scan_apk(Path(arguments["apk_path"]))
            return [TextContent(
                type="text",
                text=str({
                    "path": result.path,
                    "engine": result.engine,
                    "unity_version": result.unity_version,
                    "architectures": result.architectures,
                    "key_files": result.key_files,
                }),
            )]

        elif name == "dump_code":
            dumper = CodeDumper(output_dir=Path(arguments.get("output_dir", "./dumps")))
            result = dumper.dump(
                Path(arguments["apk_path"]),
                arguments.get("tool", "auto"),
            )
            return [TextContent(type="text", text=f"Dumped to: {result}")]

        elif name == "hook_method":
            hooker = RuntimeHooker()
            success = hooker.hook(
                package_name=arguments["package_name"],
                class_name=arguments["class_name"],
                method_name=arguments["method_name"],
                spawn=arguments.get("spawn", True),
            )
            return [TextContent(
                type="text",
                text=f"Hook {'succeeded' if success else 'failed'}",
            )]

        elif name == "list_processes":
            hooker = RuntimeHooker()
            processes = hooker.list_processes()
            return [TextContent(type="text", text="\n".join(processes))]

        elif name == "extract_assets":
            extractor = AssetExtractor(output_dir=Path(arguments.get("output_dir", "./assets")))
            result = extractor.extract(Path(arguments["apk_path"]))
            return [TextContent(type="text", text=f"Extracted to: {result}")]

        else:
            return [TextContent(type="text", text=f"Unknown tool: {name}")]

    return server


async def run_server() -> None:
    """Run the MCP server."""
    server = create_server()
    from mcp.server.stdio import stdio_server

    async with stdio_server() as (read_stream, write_stream):
        await server.run(
            read_stream,
            write_stream,
            server.create_initialization_options(),
        )


def main() -> None:
    """Entry point for the MCP server."""
    asyncio.run(run_server())


if __name__ == "__main__":
    main()
