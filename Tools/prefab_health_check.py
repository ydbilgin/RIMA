"""
RIMA Prefab Health Checker
──────────────────────────
Unity .prefab dosyalarını YAML olarak parse edip bilinen sorunları tespit eder.
Claude müdahalesine gerek kalmadan otomatik çalışır.

Kullanım:
    python Tools/prefab_health_check.py              # Sadece kontrol
    python Tools/prefab_health_check.py --fix        # Basit sorunları otomatik düzelt
    python Tools/prefab_health_check.py --verbose    # Detaylı çıktı

Tespit edilen sorunlar:
    1. SpriteRenderer sorting layer "Default" -> zeminin altında kalır (GÖRÜNMEZ)
    2. PlaceholderSprite eksik + sprite null -> MOR KARE
    3. InteractRadius çok küçük -> G tuşu çalışmaz
    4. Health component eksik -> oda temizlenmez
    5. Prefab referans bozuk (fileID: 0) -> null referans
"""

import os
import re
import sys
import yaml
from pathlib import Path
from dataclasses import dataclass, field
from typing import Optional

# ── Proje kök dizini ─────────────────────────────────────────────────────
PROJECT_ROOT = Path(__file__).resolve().parent.parent
ASSETS = PROJECT_ROOT / "Assets"
SETTINGS = PROJECT_ROOT / "ProjectSettings"

# ── Sorting Layer Haritası (TagManager.asset'ten) ────────────────────────
def load_sorting_layers() -> dict:
    """ProjectSettings/TagManager.asset'ten sorting layer sırasını oku."""
    tag_manager = SETTINGS / "TagManager.asset"
    if not tag_manager.exists():
        return {"Default": 0}
    
    layers = {"Default": 0}
    with open(tag_manager, "r", encoding="utf-8") as f:
        content = f.read()
    
    # Unity YAML: m_SortingLayers altında name ve uniqueID
    in_sorting = False
    order = 1
    for line in content.split("\n"):
        if "m_SortingLayers:" in line:
            in_sorting = True
            continue
        if in_sorting:
            if line.strip().startswith("- name:"):
                name = line.split("name:")[-1].strip()
                layers[name] = order
                order += 1
            elif not line.startswith("  ") and line.strip() and not line.strip().startswith("-"):
                break
    
    return layers


# ── Sonuç Sınıfları ──────────────────────────────────────────────────────
@dataclass
class Issue:
    prefab_path: str
    severity: str  # "ERROR", "WARNING", "INFO"
    category: str
    message: str
    auto_fixable: bool = False
    fix_description: str = ""


@dataclass
class CheckResult:
    issues: list = field(default_factory=list)
    passed: int = 0
    checked: int = 0


# ── YAML Loader (Unity custom tags handle) ───────────────────────────────
class UnityYAMLLoader(yaml.SafeLoader):
    pass

# Unity YAML'da !u!114 gibi taglar var, bunları ignore et
def unity_constructor(loader, tag_suffix, node):
    if isinstance(node, yaml.MappingNode):
        return loader.construct_mapping(node, deep=True)
    elif isinstance(node, yaml.SequenceNode):
        return loader.construct_sequence(node, deep=True)
    return loader.construct_scalar(node)

UnityYAMLLoader.add_multi_constructor("!u!", unity_constructor)
UnityYAMLLoader.add_multi_constructor("tag:unity3d.com,2011:", unity_constructor)


def parse_prefab(path: Path) -> list:
    """Unity prefab YAML'ını parse et. Birden fazla YAML document döner."""
    with open(path, "r", encoding="utf-8") as f:
        content = f.read()
    
    # Unity header: %YAML 1.1 / %TAG !u! tag:unity3d.com,2011:
    # Her component "--- !u!XXX &YYYY" ile başlar
    docs = []
    # Split by document separator
    raw_docs = re.split(r"^--- !u!\d+ &\d+.*$", content, flags=re.MULTILINE)
    headers = re.findall(r"^--- !u!(\d+) &(\d+).*$", content, flags=re.MULTILINE)
    
    for i, (type_id, file_id) in enumerate(headers):
        doc_content = raw_docs[i + 1] if i + 1 < len(raw_docs) else ""
        try:
            data = yaml.load(doc_content, Loader=UnityYAMLLoader)
            if data:
                docs.append({
                    "type_id": int(type_id),
                    "file_id": int(file_id),
                    "data": data
                })
        except yaml.YAMLError:
            pass  # Bazı Unity YAML'ları karmaşık olabilir
    
    return docs


# ── Kontrol Fonksiyonları ─────────────────────────────────────────────────

def check_sorting_layer(docs: list, path: str, sorting_layers: dict) -> list:
    """SpriteRenderer'ın sorting layer'ını kontrol et."""
    issues = []
    ground_order = sorting_layers.get("Ground", 1)
    
    for doc in docs:
        data = doc["data"]
        if "SpriteRenderer" not in data:
            continue
        
        sr = data["SpriteRenderer"]
        layer_name = sr.get("m_SortingLayerID", None)
        sorting_order = sr.get("m_SortingOrder", 0)
        
        # Sorting layer ID'den isim çözümlemesi yapmak zor,
        # ama m_SortingLayer field'ı varsa kullanabiliriz
        sorting_layer = sr.get("m_SortingLayer", 0)
        
        # Default layer (0) kontrolü
        if sorting_layer == 0:
            issues.append(Issue(
                prefab_path=path,
                severity="ERROR",
                category="SORTING_LAYER",
                message=f"SpriteRenderer 'Default' sorting layer'da! Ground layer'ın ALTINDA render edilir -> GÖRÜNMEZ!",
                auto_fixable=True,
                fix_description="Sorting layer'ı 'Entities' (3) olarak değiştir"
            ))
    
    return issues


def check_placeholder_sprite(docs: list, path: str) -> list:
    """SpriteRenderer var + sprite null + PlaceholderSprite yok = MOR KARE."""
    issues = []
    
    has_sr = False
    has_sprite = False
    has_placeholder = False
    
    for doc in docs:
        data = doc["data"]
        
        if "SpriteRenderer" in data:
            has_sr = True
            sr = data["SpriteRenderer"]
            sprite_ref = sr.get("m_Sprite", {})
            if isinstance(sprite_ref, dict):
                file_id = sprite_ref.get("fileID", 0)
                has_sprite = file_id != 0
            else:
                has_sprite = sprite_ref is not None and sprite_ref != 0
        
        if "MonoBehaviour" in data:
            mb = data["MonoBehaviour"]
            script_ref = mb.get("m_Script", {})
            # PlaceholderSprite script'ini GUID ile tanıyabiliriz
            # Ama basit yaklaşım: "shape" ve "pixelSize" field'ları varsa PlaceholderSprite
            if "shape" in mb and "pixelSize" in mb:
                has_placeholder = True
    
    if has_sr and not has_sprite and not has_placeholder:
        issues.append(Issue(
            prefab_path=path,
            severity="ERROR",
            category="MAGENTA_SQUARE",
            message="SpriteRenderer VAR ama sprite NULL ve PlaceholderSprite YOK -> MOR KARE görünür!",
            auto_fixable=False,
            fix_description="PlaceholderSprite component ekle (Unity MCP gerekli)"
        ))
    
    return issues


def check_interact_radius(docs: list, path: str) -> list:
    """RewardPickup/MapFragment interactRadius kontrolü."""
    issues = []
    
    for doc in docs:
        data = doc["data"]
        if "MonoBehaviour" not in data:
            continue
        
        mb = data["MonoBehaviour"]
        
        # interactRadius field'ı varsa kontrol et
        if "interactRadius" in mb:
            radius = mb["interactRadius"]
            if isinstance(radius, (int, float)) and radius < 3.0:
                issues.append(Issue(
                    prefab_path=path,
                    severity="ERROR",
                    category="INTERACT_RADIUS",
                    message=f"interactRadius={radius} — izometrik grid'de en az 3.0 olmalı, yoksa G tuşu çalışmaz!",
                    auto_fixable=True,
                    fix_description=f"interactRadius'u 5.0'a yükselt"
                ))
    
    return issues


def check_health_component(docs: list, path: str) -> list:
    """Enemy prefab'larda Health component olmalı."""
    issues = []
    
    # Sadece enemy prefab'lar için
    if "Enemies" not in path:
        return issues
    if "Projectile" in path:
        return issues
    
    has_health = False
    for doc in docs:
        data = doc["data"]
        if "MonoBehaviour" not in data:
            continue
        mb = data["MonoBehaviour"]
        # Health component: maxHP field'ı varsa
        if "maxHP" in mb:
            has_health = True
            break
    
    if not has_health:
        issues.append(Issue(
            prefab_path=path,
            severity="ERROR",
            category="MISSING_HEALTH",
            message="Enemy prefab'da Health component YOK -> OnEnemyDied tetiklenmez -> oda ASLA temizlenmez!",
            auto_fixable=False,
            fix_description="Health component ekle (Unity MCP gerekli)"
        ))
    
    return issues


def check_null_references(docs: list, path: str) -> list:
    """Kritik prefab referanslarında fileID: 0 kontrolü."""
    issues = []
    
    for doc in docs:
        data = doc["data"]
        if "MonoBehaviour" not in data:
            continue
        
        mb = data["MonoBehaviour"]
        
        # Bilinen kritik referanslar
        critical_refs = ["rewardPickupPrefab", "mapFragmentPrefab", "m_Script"]
        
        for ref_name in critical_refs:
            if ref_name in mb:
                ref = mb[ref_name]
                if isinstance(ref, dict) and ref.get("fileID", 1) == 0 and ref.get("guid", "") == "":
                    issues.append(Issue(
                        prefab_path=path,
                        severity="WARNING",
                        category="NULL_REFERENCE",
                        message=f"'{ref_name}' referansı null (fileID: 0) -> runtime'da çalışmaz!",
                        auto_fixable=False,
                        fix_description=f"Unity Editor'den {ref_name} referansını ata"
                    ))
    
    return issues


# ── Auto-Fix ──────────────────────────────────────────────────────────────

def auto_fix_sorting_layer(path: Path) -> bool:
    """Prefab YAML'da sorting layer'ı Entities (3) olarak değiştir."""
    with open(path, "r", encoding="utf-8") as f:
        content = f.read()
    
    # m_SortingLayer: 0 -> m_SortingLayer: 3 (Entities)
    new_content = re.sub(
        r"(m_SortingLayer:\s*)0",
        r"\g<1>3",
        content
    )
    
    if new_content != content:
        with open(path, "w", encoding="utf-8") as f:
            f.write(new_content)
        return True
    return False


def auto_fix_interact_radius(path: Path, new_radius: float = 5.0) -> bool:
    """interactRadius değerini yükselt."""
    with open(path, "r", encoding="utf-8") as f:
        content = f.read()
    
    new_content = re.sub(
        r"(interactRadius:\s*)\d+\.?\d*",
        f"\\g<1>{new_radius}",
        content
    )
    
    if new_content != content:
        with open(path, "w", encoding="utf-8") as f:
            f.write(new_content)
        return True
    return False


# ── Ana Çalıştırıcı ──────────────────────────────────────────────────────

def scan_prefabs(fix: bool = False, verbose: bool = False) -> CheckResult:
    """Tüm prefab'ları tara ve sorunları raporla."""
    result = CheckResult()
    sorting_layers = load_sorting_layers()
    
    if verbose:
        print(f"[DIR] Proje: {PROJECT_ROOT}")
        print(f"[LAYERS] Sorting Layers: {sorting_layers}")
        print()
    
    # Taranacak prefab dizinleri
    prefab_dirs = [
        ASSETS / "Prefabs",
    ]
    
    for prefab_dir in prefab_dirs:
        if not prefab_dir.exists():
            continue
        
        for prefab_file in prefab_dir.rglob("*.prefab"):
            result.checked += 1
            rel_path = str(prefab_file.relative_to(PROJECT_ROOT))
            
            if verbose:
                print(f"  [SCAN] {rel_path}")
            
            try:
                docs = parse_prefab(prefab_file)
            except Exception as e:
                result.issues.append(Issue(
                    prefab_path=rel_path,
                    severity="WARNING",
                    category="PARSE_ERROR",
                    message=f"YAML parse hatası: {e}"
                ))
                continue
            
            # Tüm kontrolleri çalıştır
            issues = []
            issues.extend(check_sorting_layer(docs, rel_path, sorting_layers))
            issues.extend(check_placeholder_sprite(docs, rel_path))
            issues.extend(check_interact_radius(docs, rel_path))
            issues.extend(check_health_component(docs, rel_path))
            issues.extend(check_null_references(docs, rel_path))
            
            if not issues:
                result.passed += 1
            
            # Auto-fix
            if fix:
                for issue in issues:
                    if issue.auto_fixable:
                        fixed = False
                        if issue.category == "SORTING_LAYER":
                            fixed = auto_fix_sorting_layer(prefab_file)
                        elif issue.category == "INTERACT_RADIUS":
                            fixed = auto_fix_interact_radius(prefab_file)
                        
                        if fixed:
                            issue.message += " -> [FIXED] OTOMATIK DUZELTILDI"
                            issue.severity = "FIXED"
            
            result.issues.extend(issues)
    
    return result


def print_report(result: CheckResult):
    """Güzel formatlanmış rapor yazdır."""
    
    COLORS = {
        "ERROR": "\033[91m",    # Kırmızı
        "WARNING": "\033[93m",  # Sarı
        "INFO": "\033[94m",     # Mavi
        "FIXED": "\033[92m",    # Yeşil
        "RESET": "\033[0m",
    }
    
    print("\n" + "=" * 60)
    print("  RIMA Prefab Health Report")
    print("=" * 60)
    print(f"  Taranan: {result.checked} prefab")
    print(f"  Temiz:   {result.passed} prefab")
    print(f"  Sorunlu: {result.checked - result.passed} prefab")
    print(f"  Toplam sorun: {len(result.issues)}")
    print("=" * 60)
    
    if not result.issues:
        print(f"\n  {COLORS['FIXED']}[OK] TUM PREFABLAR SAGLIKLI!{COLORS['RESET']}\n")
        return
    
    # Kategoriye göre grupla
    by_category = {}
    for issue in result.issues:
        by_category.setdefault(issue.category, []).append(issue)
    
    for category, issues in by_category.items():
        color = COLORS.get(issues[0].severity, COLORS["RESET"])
        print(f"\n  {color}[{category}]{COLORS['RESET']}")
        for issue in issues:
            sev_color = COLORS.get(issue.severity, COLORS["RESET"])
            prefix = "[X]" if issue.severity == "ERROR" else "[!]" if issue.severity == "WARNING" else "[OK]"
            print(f"    {prefix} {issue.prefab_path}")
            print(f"       {sev_color}{issue.message}{COLORS['RESET']}")
            if issue.fix_description and issue.severity != "FIXED":
                print(f"       -> Fix: {issue.fix_description}")
    
    # Escalation özeti
    needs_claude = [i for i in result.issues if not i.auto_fixable and i.severity == "ERROR"]
    if needs_claude:
        print(f"\n  {COLORS['WARNING']}[!] {len(needs_claude)} sorun Claude mudahalesi gerektirir:{COLORS['RESET']}")
        for issue in needs_claude:
            print(f"    -> {issue.prefab_path}: {issue.category}")
    
    print()


# ── Entry Point ──────────────────────────────────────────────────────────

if __name__ == "__main__":
    fix_mode = "--fix" in sys.argv
    verbose = "--verbose" in sys.argv
    
    if fix_mode:
        print("[FIX MODE] Otomatik duzeltilebilir sorunlar fix edilecek\n")
    
    result = scan_prefabs(fix=fix_mode, verbose=verbose)
    print_report(result)
    
    # Exit code: sorun varsa 1
    errors = [i for i in result.issues if i.severity == "ERROR"]
    sys.exit(1 if errors else 0)
