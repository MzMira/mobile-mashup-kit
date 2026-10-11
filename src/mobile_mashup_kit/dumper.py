"""Code dumper for extracting readable code from APKs."""
import subprocess
import shutil
import zipfile
from pathlib import Path
from typing import Optional
from .scanner import scan_apk, ScanResult


class CodeDumper:
    """Dumps readable code from an APK based on the detected engine."""

    def __init__(self, output_dir: Path):
        self.output_dir = Path(output_dir)
        self.output_dir.mkdir(parents=True, exist_ok=True)

    def dump(self, apk_path: Path, tool: str = "auto") -> Path:
        """Dump code from an APK, auto-detecting the engine first."""
        scan_result = scan_apk(apk_path)

        if scan_result.engine == "unity-il2cpp":
            return self._dump_unity_il2cpp(apk_path, scan_result, tool)
        elif scan_result.engine == "unity-mono":
            return self._dump_unity_mono(apk_path, scan_result)
        elif scan_result.engine == "cocos-lua":
            return self._dump_cocos_lua(apk_path, scan_result)
        else:
            return self._dump_generic(apk_path, scan_result)

    def _dump_unity_il2cpp(self, apk_path: Path, scan_result: ScanResult, tool: str) -> Path:
        """Dump Unity IL2CPP code using Il2CppInspector or Il2CppDumper."""
        out = self.output_dir / "il2cpp"
        out.mkdir(exist_ok=True)

        if tool in ("auto", "il2cppinspector") and shutil.which("Il2CppInspector"):
            cmd = [
                "Il2CppInspector",
                "-i", str(apk_path),
                "-o", str(out),
            ]
            subprocess.run(cmd, capture_output=True, timeout=600)
        elif tool in ("auto", "il2cppdumper") and shutil.which("Il2CppDumper"):
            cmd = [
                "Il2CppDumper",
                str(apk_path),
                str(out),
            ]
            subprocess.run(cmd, capture_output=True, timeout=600)

        return out

    def _dump_unity_mono(self, apk_path: Path, scan_result: ScanResult) -> Path:
        """Dump Unity Mono code using jadx."""
        out = self.output_dir / "mono"
        out.mkdir(exist_ok=True)

        if shutil.which("jadx"):
            cmd = [
                "jadx",
                "-d", str(out),
                str(apk_path),
            ]
            subprocess.run(cmd, capture_output=True, timeout=600)

        return out

    def _dump_cocos_lua(self, apk_path: Path, scan_result: ScanResult) -> Path:
        """Dump Cocos2d-x Lua code using unluac."""
        out = self.output_dir / "lua"
        out.mkdir(exist_ok=True)

        if shutil.which("unluac"):
            cmd = [
                "unluac",
                "-o", str(out),
                str(apk_path),
            ]
            subprocess.run(cmd, capture_output=True, timeout=600)

        return out

    def _dump_generic(self, apk_path: Path, scan_result: ScanResult) -> Path:
        """Generic fallback - just extract the APK."""
        out = self.output_dir / "generic"
        out.mkdir(exist_ok=True)

        if apk_path.is_file() and apk_path.suffix.lower() in [".apk", ".apkm", ".xapk"]:
            with zipfile.ZipFile(apk_path, 'r') as z:
                z.extractall(out)
        elif apk_path.is_dir():
            import shutil
            for f in apk_path.rglob("*"):
                if f.is_file():
                    rel = f.relative_to(apk_path)
                    target = out / rel
                    target.parent.mkdir(parents=True, exist_ok=True)
                    shutil.copy2(f, target)

        return out
