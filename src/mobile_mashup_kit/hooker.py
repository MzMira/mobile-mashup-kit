"""Runtime hooker for intercepting method calls in running games."""
import subprocess
import shutil
from pathlib import Path
from typing import Optional, Callable
from .scanner import ScanResult


class RuntimeHooker:
    """Hooks into a running game process using Frida."""

    def __init__(self):
        self._check_frida()

    def _check_frida(self):
        """Check if frida-tools is available."""
        if not shutil.which("frida"):
            raise RuntimeError(
                "frida-tools not found. Install with: pip install frida-tools"
            )

    def hook(
        self,
        package_name: str,
        class_name: str,
        method_name: str,
        callback: Optional[Callable] = None,
        spawn: bool = True,
    ) -> bool:
        """Hook a method in a running game.

        Args:
            package_name: The game's package name (e.g., "com.kakaogames.gdts")
            class_name: The class to hook
            method_name: The method to hook
            callback: Optional callback function for hook events
            spawn: Whether to spawn the process or attach to it

        Returns:
            True if hook was successful
        """
        # Generate Frida script
        script = self._generate_hook_script(class_name, method_name, callback)

        # Write script to temp file
        import tempfile
        with tempfile.NamedTemporaryFile(mode='w', suffix='.js', delete=False) as f:
            f.write(script)
            script_path = f.name

        try:
            # Run frida
            cmd = ["frida", "-U"]
            if spawn:
                cmd.extend(["-f", package_name])
            else:
                cmd.extend(["-n", package_name])
            cmd.extend(["-l", script_path])

            result = subprocess.run(cmd, capture_output=True, text=True, timeout=60)
            return result.returncode == 0
        finally:
            Path(script_path).unlink(missing_ok=True)

    def _generate_hook_script(
        self,
        class_name: str,
        method_name: str,
        callback: Optional[Callable] = None,
    ) -> str:
        """Generate a Frida script for hooking."""
        script = f"""
Java.perform(function() {{
    var targetClass = Java.use("{class_name}");
    var originalMethod = targetClass.{method_name};
    
    targetClass.{method_name}.implementation = function() {{
        console.log("[Hook] {class_name}.{method_name} called");
        console.log("[Hook] Arguments: " + JSON.stringify(arguments));
        
        var result = originalMethod.apply(this, arguments);
        
        console.log("[Hook] Return value: " + result);
        return result;
    }};
}});
"""
        return script

    def list_processes(self) -> list[str]:
        """List running processes on the device."""
        result = subprocess.run(
            ["frida-ps", "-U"],
            capture_output=True,
            text=True,
            timeout=30,
        )
        if result.returncode == 0:
            return result.stdout.strip().split('\n')
        return []
