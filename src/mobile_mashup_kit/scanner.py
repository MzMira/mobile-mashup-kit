"""APK scanner for engine detection."""
import zipfile
from pathlib import Path
from dataclasses import dataclass
from typing import Optional


@dataclass
class ScanResult:
    """Result of scanning an APK."""
    path: str
    engine: Optional[str]  # "unity-il2cpp", "unity-mono", "cocos-lua", "unknown"
    unity_version: Optional[str]
    architectures: list[str]
    key_files: list[str]
    is_apk: bool
    is_dir: bool


def scan_apk(path: Path) -> ScanResult:
    """Scan an APK or decompiled directory to detect the game engine."""
    result = ScanResult(
        path=str(path),
        engine=None,
        unity_version=None,
        architectures=[],
        key_files=[],
        is_apk=path.suffix.lower() in [".apk", ".apkm", ".xapk"],
        is_dir=path.is_dir(),
    )

    if result.is_apk:
        _scan_apk_file(path, result)
    elif result.is_dir:
        _scan_directory(path, result)

    return result


def _scan_apk_file(path: Path, result: ScanResult) -> None:
    """Scan an APK file."""
    try:
        with zipfile.ZipFile(path, "r") as z:
            names = z.namelist()
            _analyze_file_list(names, result)
    except zipfile.BadZipFile:
        pass


def _scan_directory(path: Path, result: ScanResult) -> None:
    """Scan a decompiled directory."""
    names = []
    for p in path.rglob("*"):
        if p.is_file():
            # Normalize to forward slashes for cross-platform consistency
            names.append(p.relative_to(path).as_posix())
    _analyze_file_list(names, result)


def _analyze_file_list(names: list[str], result: ScanResult) -> None:
    """Analyze a list of file paths to detect the engine."""
    # Check for Unity IL2CPP
    has_libil2cpp = any("libil2cpp.so" in n for n in names)
    has_global_metadata = any("global-metadata.dat" in n for n in names)
    has_managed = any("Managed/" in n for n in names)

    # Check for Unity Mono
    has_assembly_csharp = any("Assembly-CSharp.dll" in n for n in names)
    has_mono = any("libmono.so" in n for n in names)

    # Check for Cocos2d-x
    has_cocos = any("libcocos2d" in n for n in names)
    has_lua = any("lua" in n.lower() for n in names)

    # Detect architectures
    for arch in ["arm64-v8a", "armeabi-v7a", "x86_64", "x86"]:
        if any(f"lib/{arch}/" in n for n in names):
            result.architectures.append(arch)

    # Determine engine
    if has_libil2cpp and has_global_metadata:
        result.engine = "unity-il2cpp"
        result.key_files = [n for n in names if "libil2cpp.so" in n or "global-metadata.dat" in n]
    elif has_assembly_csharp or (has_managed and has_mono):
        result.engine = "unity-mono"
        result.key_files = [n for n in names if "Assembly-CSharp.dll" in n or "libmono.so" in n]
    elif has_cocos or (has_lua and not has_managed):
        result.engine = "cocos-lua"
        result.key_files = [n for n in names if "cocos" in n.lower() or "lua" in n.lower()]
    else:
        result.engine = "unknown"

    # Try to detect Unity version from global-metadata.dat
    if has_global_metadata:
        result.unity_version = "detected"  # Would need to parse the file
