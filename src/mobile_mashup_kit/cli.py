"""CLI for Mobile Mashup Kit."""
import json
import sys
from pathlib import Path

import click

from .scanner import scan_apk, ScanResult
from .dumper import CodeDumper
from .hooker import RuntimeHooker


def _print_scan_result(result: ScanResult) -> None:
    """Print a scan result in human-readable format."""
    click.echo(f"Path: {result.path}")
    click.echo(f"Engine: {result.engine or 'unknown'}")
    if result.unity_version:
        click.echo(f"Unity version: {result.unity_version}")
    if result.architectures:
        click.echo(f"Architectures: {', '.join(result.architectures)}")
    if result.key_files:
        click.echo("Key files:")
        for f in result.key_files[:10]:
            click.echo(f"  - {f}")
        if len(result.key_files) > 10:
            click.echo(f"  ... and {len(result.key_files) - 10} more")


@click.group()
@click.version_option(version="0.1.0", prog_name="mum")
def cli() -> None:
    """Mobile Mashup Kit - AI-assisted game modding toolkit for mobile."""
    pass


@cli.command()
@click.argument("apk_path", type=click.Path(exists=True, dir_okay=True))
@click.option("--json", "output_json", is_flag=True, help="Output as JSON")
def scan(apk_path: str, output_json: bool) -> None:
    """Scan an APK or decompiled directory to detect the game engine."""
    result = scan_apk(Path(apk_path))
    if output_json:
        click.echo(json.dumps({
            "path": result.path,
            "engine": result.engine,
            "unity_version": result.unity_version,
            "architectures": result.architectures,
            "key_files": result.key_files,
        }, indent=2))
    else:
        _print_scan_result(result)


@cli.command()
@click.argument("apk_path", type=click.Path(exists=True, dir_okay=True))
@click.option("-o", "--output", default="./dumps", help="Output directory")
@click.option(
    "--tool",
    type=click.Choice(["auto", "il2cppdumper", "il2cppinspector", "jadx", "unluac"]),
    default="auto",
    help="Dumper tool to use",
)
def dump(apk_path: str, output: str, tool: str) -> None:
    """Dump readable code from an APK."""
    dumper = CodeDumper(output_dir=Path(output))
    result = dumper.dump(Path(apk_path), tool)
    click.echo(f"Dumped to: {result}")


@cli.command()
@click.option("--package", required=True, help="Package name")
@click.option("--class", "class_name", required=True, help="Class name")
@click.option("--method", required=True, help="Method name")
@click.option("--no-spawn", is_flag=True, help="Attach instead of spawn")
def hook(package: str, class_name: str, method: str, no_spawn: bool) -> None:
    """Hook a method in a running game."""
    hooker = RuntimeHooker()
    success = hooker.hook(
        package_name=package,
        class_name=class_name,
        method_name=method,
        spawn=not no_spawn,
    )
    if success:
        click.echo(f"Hooked {class_name}.{method}")
    else:
        click.echo("Hook failed", err=True)
        sys.exit(1)


@cli.command(name="ps")
def ps() -> None:
    """List running processes on the device."""
    hooker = RuntimeHooker()
    processes = hooker.list_processes()
    for proc in processes:
        click.echo(proc)


@cli.command()
@click.argument("apk_path", type=click.Path(exists=True, dir_okay=True))
@click.option("-o", "--output", default="./assets", help="Output directory")
def extract(apk_path: str, output: str) -> None:
    """Extract assets from an APK."""
    from .extractor import AssetExtractor
    extractor = AssetExtractor(output_dir=Path(output))
    result = extractor.extract(Path(apk_path))
    click.echo(f"Extracted to: {result}")


def main() -> None:
    """Entry point for the mum CLI."""
    cli()
