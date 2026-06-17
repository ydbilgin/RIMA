# -*- coding: utf-8 -*-
"""RIMA — Sunum Kilavuzu v2 (uzun + guzel tasarim) A4 PDF uretici.

Kaynak icerik: STAGING/report/BUILDER_SUNUM_KILAVUZU_V2.md (AYNEN kullanildi).
reportlab 4.5.1 ile formatlanir. cx KULLANILMADI, git commit YOK.

Tasarim: teal banner baslik, numarali adim kartlari (GOSTER/SOYLE/TUS/DIKKAT
renk-kodlu), acilis+kapanis konusma kutulari, guclu cumleler accent-bar,
alternating-row tablolar, checklist, amber fallback kutusu, 2 gomulu gorsel,
footer sayfa no.
"""
import os
from reportlab.lib.pagesizes import A4
from reportlab.lib.units import mm
from reportlab.lib import colors
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.enums import TA_LEFT, TA_CENTER
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont
from reportlab.platypus import (
    SimpleDocTemplate, Paragraph, Spacer, Table, TableStyle,
    ListFlowable, ListItem, HRFlowable, Image, KeepTogether, Flowable,
)

HERE = os.path.dirname(os.path.abspath(__file__))
FIG_DIR = os.path.join(HERE, "figures_2026-06-18")

# ---- Font kaydi: tam Turkce karakter (Segoe UI; yoksa DejaVuSans) ----
BASE_FONT = "Body"
BOLD_FONT = "BodyBold"
ITALIC_FONT = "BodyItalic"
MONO_FONT = "Mono"


def register_fonts():
    win = "C:/Windows/Fonts"
    if os.path.exists(win + "/segoeui.ttf"):
        pdfmetrics.registerFont(TTFont(BASE_FONT, win + "/segoeui.ttf"))
        pdfmetrics.registerFont(TTFont(BOLD_FONT, win + "/segoeuib.ttf"))
        ital = win + "/segoeuii.ttf"
        pdfmetrics.registerFont(
            TTFont(ITALIC_FONT, ital if os.path.exists(ital)
                   else win + "/segoeui.ttf"))
        mono = win + "/consola.ttf"
        pdfmetrics.registerFont(
            TTFont(MONO_FONT, mono if os.path.exists(mono)
                   else win + "/segoeui.ttf"))
        return "Segoe UI"
    # Linux / fallback
    dv = "/usr/share/fonts/truetype/dejavu"
    if os.path.exists(dv + "/DejaVuSans.ttf"):
        pdfmetrics.registerFont(TTFont(BASE_FONT, dv + "/DejaVuSans.ttf"))
        pdfmetrics.registerFont(TTFont(BOLD_FONT, dv + "/DejaVuSans-Bold.ttf"))
        pdfmetrics.registerFont(TTFont(ITALIC_FONT, dv + "/DejaVuSans-Oblique.ttf"))
        pdfmetrics.registerFont(TTFont(MONO_FONT, dv + "/DejaVuSansMono.ttf"))
        return "DejaVuSans"
    for nm in (BASE_FONT, BOLD_FONT, ITALIC_FONT, MONO_FONT):
        pdfmetrics.registerFont(TTFont(nm, "DejaVuSans"))
    return "DejaVuSans (fallback)"


used_font = register_fonts()

# ---- Renkler ----
TEAL = colors.HexColor("#21576B")
TEAL_DARK = colors.HexColor("#173E4D")
TEAL_LIGHT = colors.HexColor("#EAF1F4")
TEAL_MID = colors.HexColor("#E3EDF1")
ROW_ALT = colors.HexColor("#F4F7F9")
GRID = colors.HexColor("#B8C9D1")
TEXT = colors.HexColor("#1F2A2E")
MUTED = colors.HexColor("#52666E")
AMBER = colors.HexColor("#B5651D")
AMBER_BG = colors.HexColor("#FBF1E2")
AMBER_BORDER = colors.HexColor("#D8A45A")
MONO_BG = colors.HexColor("#EEF1F3")
MONO_TXT = colors.HexColor("#3C4B51")

# ---- Stiller ----
styles = getSampleStyleSheet()

subtitle_style = ParagraphStyle(
    "RSubtitle", parent=styles["Normal"], fontName=BASE_FONT,
    fontSize=10.5, leading=13, textColor=MUTED, spaceAfter=4,
)
h2_style = ParagraphStyle(
    "RH2", parent=styles["Heading2"], fontName=BOLD_FONT,
    fontSize=13.5, leading=16, textColor=TEAL, spaceBefore=12, spaceAfter=4,
)
body_style = ParagraphStyle(
    "RBody", parent=styles["Normal"], fontName=BASE_FONT,
    fontSize=10, leading=13.5, textColor=TEXT, spaceAfter=3,
)
bullet_style = ParagraphStyle(
    "RBullet", parent=body_style, fontSize=10, leading=13.5,
)
cell_style = ParagraphStyle(
    "RCell", parent=styles["Normal"], fontName=BASE_FONT,
    fontSize=9, leading=11.8, textColor=TEXT,
)
cell_bold = ParagraphStyle(
    "RCellBold", parent=cell_style, fontName=BOLD_FONT, textColor=TEAL,
)
cell_head = ParagraphStyle(
    "RCellHead", parent=cell_style, fontName=BOLD_FONT,
    textColor=colors.white,
)
q_style = ParagraphStyle(
    "RQ", parent=body_style, fontName=BOLD_FONT, textColor=TEAL,
    fontSize=10, leading=13, spaceBefore=5, spaceAfter=0,
)
a_style = ParagraphStyle(
    "RA", parent=body_style, fontSize=9.5, leading=12.5,
    leftIndent=12, spaceAfter=3,
)
speech_style = ParagraphStyle(
    "RSpeech", parent=body_style, fontName=ITALIC_FONT,
    fontSize=10.5, leading=15, textColor=TEAL_DARK,
)
caption_style = ParagraphStyle(
    "RCaption", parent=body_style, fontName=ITALIC_FONT,
    fontSize=8, leading=10, textColor=MUTED, alignment=TA_CENTER,
    spaceBefore=2, spaceAfter=4,
)
checklist_style = ParagraphStyle(
    "RCheck", parent=body_style, fontSize=10, leading=16,
)
strong_style = ParagraphStyle(
    "RStrong", parent=body_style, fontName=BOLD_FONT, fontSize=10,
    leading=13.5, textColor=TEAL_DARK, leftIndent=10,
    spaceBefore=2, spaceAfter=2,
)
# Step card sub-styles
card_title_style = ParagraphStyle(
    "RCardTitle", parent=body_style, fontName=BOLD_FONT, fontSize=11,
    leading=13.5, textColor=TEAL_DARK, spaceAfter=2,
)
card_line_style = ParagraphStyle(
    "RCardLine", parent=body_style, fontSize=9.5, leading=12.5, spaceAfter=1,
)


def b(text):
    return '<font name="%s">%s</font>' % (BOLD_FONT, text)


def section_rule():
    return HRFlowable(width="100%", thickness=1.1, color=TEAL,
                      spaceBefore=2, spaceAfter=6)


def bulleted(items, color=TEAL, indent=14):
    li = [ListItem(Paragraph(t, bullet_style)) for t in items]
    return ListFlowable(li, bulletType="bullet", bulletColor=color,
                        leftIndent=indent, bulletFontSize=8)


# ---- Banner basligi (canvas-cizimli teal bar) ----
class Banner(Flowable):
    def __init__(self, width, title, subtitle, height=22 * mm):
        super().__init__()
        self.width = width
        self.h = height
        self.title = title
        self.subtitle = subtitle

    def wrap(self, aw, ah):
        return self.width, self.h

    def draw(self):
        c = self.canv
        c.saveState()
        c.setFillColor(TEAL)
        c.roundRect(0, 0, self.width, self.h, 4, fill=1, stroke=0)
        # ince koyu sol seridi (accent)
        c.setFillColor(TEAL_DARK)
        c.roundRect(0, 0, 3 * mm, self.h, 4, fill=1, stroke=0)
        c.setFillColor(colors.white)
        c.setFont(BOLD_FONT, 17)
        c.drawString(7 * mm, self.h - 9.5 * mm, self.title)
        c.setFont(BASE_FONT, 9)
        c.setFillColor(colors.HexColor("#CFE0E7"))
        c.drawString(7 * mm, self.h - 16 * mm, self.subtitle)
        c.restoreState()


# ---- Numarali adim karti ----
def step_card(num, title, goster=None, soyle=None, tus=None, dikkat=None,
              content_w=174 * mm):
    """Sol teal kare numara + sag etiketli satirlar -> 1x2 Table."""
    badge = Table([[Paragraph(
        '<font color="#FFFFFF" name="%s" size="14">%d</font>' % (BOLD_FONT, num),
        ParagraphStyle("b", alignment=TA_CENTER))]],
        colWidths=[9 * mm], rowHeights=[9 * mm])
    badge.setStyle(TableStyle([
        ("BACKGROUND", (0, 0), (-1, -1), TEAL),
        ("VALIGN", (0, 0), (-1, -1), "MIDDLE"),
        ("ALIGN", (0, 0), (-1, -1), "CENTER"),
        ("LEFTPADDING", (0, 0), (-1, -1), 0),
        ("RIGHTPADDING", (0, 0), (-1, -1), 0),
        ("TOPPADDING", (0, 0), (-1, -1), 0),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 0),
        ("ROUNDEDCORNERS", [3, 3, 3, 3]),
    ]))

    right = [Paragraph(title, card_title_style)]
    if goster:
        right.append(Paragraph(
            '<font color="#21576B" name="%s">GÖSTER:</font> %s'
            % (BOLD_FONT, goster), card_line_style))
    if soyle:
        right.append(Paragraph(
            '<font color="#21576B" name="%s">SÖYLE:</font> '
            '<font name="%s" color="#173E4D">“%s”</font>'
            % (BOLD_FONT, ITALIC_FONT, soyle), card_line_style))
    if tus:
        right.append(Paragraph(
            '<font color="#21576B" name="%s">TUŞ:</font> '
            '<font name="%s" color="#3C4B51">%s</font>'
            % (BOLD_FONT, MONO_FONT, tus), card_line_style))
    if dikkat:
        right.append(Paragraph(
            '<font color="#B5651D" name="%s">DİKKAT:</font> '
            '<font color="#B5651D">%s</font>'
            % (BOLD_FONT, dikkat), card_line_style))

    inner = Table([[badge, right]], colWidths=[12 * mm, content_w - 12 * mm])
    inner.setStyle(TableStyle([
        ("VALIGN", (0, 0), (0, 0), "TOP"),
        ("VALIGN", (1, 0), (1, 0), "TOP"),
        ("LEFTPADDING", (0, 0), (-1, -1), 0),
        ("RIGHTPADDING", (0, 0), (-1, -1), 0),
        ("TOPPADDING", (0, 0), (-1, -1), 2),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 2),
        ("LINEBELOW", (0, 0), (-1, -1), 0.5, GRID),
    ]))
    return KeepTogether([inner, Spacer(1, 3)])


# ---- Konusma kutusu (acik-teal arka plan) ----
def speech_box(text, w=174 * mm):
    p = Paragraph(text, speech_style)
    t = Table([[p]], colWidths=[w])
    t.setStyle(TableStyle([
        ("BACKGROUND", (0, 0), (-1, -1), TEAL_LIGHT),
        ("BOX", (0, 0), (-1, -1), 0.8, TEAL),
        ("LINEBEFORE", (0, 0), (0, -1), 3, TEAL),
        ("LEFTPADDING", (0, 0), (-1, -1), 9),
        ("RIGHTPADDING", (0, 0), (-1, -1), 9),
        ("TOPPADDING", (0, 0), (-1, -1), 7),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 7),
    ]))
    return t


# ---- Amber fallback kutusu ----
def amber_box(rows, w=174 * mm):
    flow = []
    for label, txt in rows:
        flow.append(Paragraph(
            '<font color="#B5651D" name="%s">► %s</font> %s'
            % (BOLD_FONT, label, txt),
            ParagraphStyle("amb", parent=body_style, fontSize=9.5,
                           leading=12.5, textColor=TEXT, spaceAfter=3)))
    t = Table([[flow]], colWidths=[w])
    t.setStyle(TableStyle([
        ("BACKGROUND", (0, 0), (-1, -1), AMBER_BG),
        ("BOX", (0, 0), (-1, -1), 0.9, AMBER_BORDER),
        ("LINEBEFORE", (0, 0), (0, -1), 3, AMBER),
        ("LEFTPADDING", (0, 0), (-1, -1), 9),
        ("RIGHTPADDING", (0, 0), (-1, -1), 9),
        ("TOPPADDING", (0, 0), (-1, -1), 7),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 5),
    ]))
    return t


# ---- Tablo (teal baslik + alternating satir) ----
def styled_table(header, body_rows, col_w, last_row_highlight=False):
    data = [[Paragraph(header[0], cell_head), Paragraph(header[1], cell_head)]]
    for r in body_rows:
        data.append([Paragraph(r[0], cell_bold), Paragraph(r[1], cell_style)])
    tbl = Table(data, colWidths=col_w, repeatRows=1)
    ts = [
        ("BACKGROUND", (0, 0), (-1, 0), TEAL),
        ("VALIGN", (0, 0), (-1, -1), "MIDDLE"),
        ("TOPPADDING", (0, 0), (-1, -1), 4),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 4),
        ("LEFTPADDING", (0, 0), (-1, -1), 6),
        ("RIGHTPADDING", (0, 0), (-1, -1), 6),
        ("GRID", (0, 0), (-1, -1), 0.6, GRID),
        ("LINEBELOW", (0, 0), (-1, 0), 1.2, TEAL_DARK),
    ]
    for i in range(1, len(data)):
        if i % 2 == 0:
            ts.append(("BACKGROUND", (0, i), (-1, i), ROW_ALT))
    if last_row_highlight:
        ts.append(("BACKGROUND", (0, len(data) - 1), (-1, len(data) - 1),
                   TEAL_MID))
    tbl.setStyle(TableStyle(ts))
    return tbl


def fig(path, width_mm, caption):
    from reportlab.lib.utils import ImageReader
    ir = ImageReader(path)
    iw, ih = ir.getSize()
    w = width_mm * mm
    h = w * ih / iw
    img = Image(path, width=w, height=h)
    img.hAlign = "CENTER"
    return KeepTogether([img, Paragraph(caption, caption_style)])


# =====================================================================
#  ICERIK (BUILDER_SUNUM_KILAVUZU_V2.md'den AYNEN)
# =====================================================================
story = []
CW = 174 * mm  # icerik genisligi (A4 - 2*18mm margin)

# ---- Banner ----
story.append(Banner(
    CW, "RIMA — RİFT AVCILARI · SUNUM KILAVUZU",
    "Sunarken yanında aç · Yasin Derya Bilgin"))
story.append(Spacer(1, 7))

# ============== 0. CHECKLIST ==============
story.append(Paragraph("0. Sunum Öncesi Checklist", h2_style))
story.append(section_rule())
checklist = [
    "Unity açık + derlenmiş (0 error)",
    "Play MainMenu’den başlıyor",
    "Ses açık (müzik + SFX)",
    "F2 (Build Mode) ve ` (Director) çalışıyor — 10 sn dene",
    "Graphify god-node görseli / slide hazır",
    "Yedek demo videosu hazır (canlı çökerse)",
    "Game view tam ekran / maximize",
    "Warblade sınıfı seçili akış prova edildi",
]
check_paras = [Paragraph("☐ " + c, checklist_style) for c in checklist]
ck_tbl = Table([[check_paras[0], check_paras[1]],
                [check_paras[2], check_paras[3]],
                [check_paras[4], check_paras[5]],
                [check_paras[6], check_paras[7]]],
               colWidths=[CW / 2, CW / 2])
ck_tbl.setStyle(TableStyle([
    ("VALIGN", (0, 0), (-1, -1), "TOP"),
    ("LEFTPADDING", (0, 0), (-1, -1), 2),
    ("TOPPADDING", (0, 0), (-1, -1), 1),
    ("BOTTOMPADDING", (0, 0), (-1, -1), 1),
]))
story.append(ck_tbl)

# ============== 1. ACILIS KONUSMASI ==============
story.append(Paragraph("1. Açılış Konuşması (~30 sn — okuyabilirsin)", h2_style))
story.append(section_rule())
story.append(fig(os.path.join(FIG_DIR, "fig_graphify_godnodes.png"), 88,
                 "Graphify god-node grafı — en bağlı 10 bileşenin 6’sı "
                 "editör/araç sınıfı (tooling tezinin sayısal kanıtı)."))
story.append(speech_box(
    "Merhaba, ben Yasin Derya Bilgin. Projem RIMA — Rift Avcıları. Bunu "
    "sadece bir oyun olarak değil; bir geliştirme environment’ı ve "
    "yeniden-kullanılabilir oyun-içi araç seti olarak tasarladım. Ekrandaki "
    "bu kod-grafında en bağlı 10 bileşenin 6’sı editör/araç sınıfı — yani "
    "bu proje içeriğiyle değil, mimarisi ve araçlarıyla değerlendirilmeli. "
    "Şimdi hem oynanabilir döngüyü hem de bu araçları göstereceğim."))

# ============== 2. DEMO AKISI (9 adim karti) ==============
story.append(Paragraph("2. Demo Akışı (9 adım — sırayla göster)", h2_style))
story.append(section_rule())
story.append(step_card(
    1, "Açılış görseli",
    goster="Graphify god-node görseli.",
    soyle="Bu bir environment + tooling, veriyle.",
    tus="—", dikkat="Konuşmayı buradan aç."))
story.append(step_card(
    2, "Menü → Karakter",
    goster="MainMenu → BAŞLA → Warblade.",
    soyle="Ana sınıf Warblade ile gidiyorum.",
    tus="Mouse"))
story.append(step_card(
    3, "Combat",
    goster="Hareket + LMB combo + Q/E/R/F, wave temizle.",
    soyle="Stat’lar hasara yansıyor; juice ile vuruş hissi var.",
    tus="WASD · LMB · Q/E/R/F",
    dikkat="Hit-stop / hasar sayısını göster."))
story.append(step_card(
    4, "Boss",
    goster="Telegraph’lar + can barı düşüşü.",
    soyle="Kırmızı tehlike, yeşil güvenli-halka; telegraph bitince hasar.",
    tus="—", dikkat="Yeşil safe-ring’i işaret et."))
story.append(step_card(
    5, "Ödül → Draft",
    goster="Kart seçimi.",
    soyle="Her oda sonrası build’ini şekillendiriyorsun.",
    tus="G + tıkla"))
story.append(step_card(
    6, "Run-map",
    goster="Branching oda ilerleme.",
    soyle="Her koşu farklı: seed’li dallanma, Merchant / Elite.",
    tus="—"))
# CENTERPIECE — gomulu gorsel ile
story.append(step_card(
    7, "⭐ CENTERPIECE — Edit-to-Play",
    goster="F2 Build Mode → prop koy / oda düzenle → çık → aynı odayı oyna.",
    soyle="Unity’yi açıp kapatmadan, oyun çalışırken seviye tasarlıyorum — "
          "projenin kalbi bu.",
    tus="F2", dikkat="En güçlü an; YAVAŞ ve NET göster."))
story.append(fig(os.path.join(FIG_DIR, "fig_buildmode_centerpiece.png"), 110,
                 "Edit-to-Play: oyun çalışırken oyun-içi seviye editörü "
                 "(Build Mode, F2) — projenin centerpiece anı."))
story.append(step_card(
    8, "Director Mode",
    goster="Stat slider ile zorluk / spawn / telemetry.",
    soyle="Canlı tuning aracı — dengelemeyi oyunu durdurmadan yapıyorum.",
    tus="` (backquote)"))
story.append(step_card(
    9, "Kapanış",
    goster="Graphify + süreç.",
    soyle="Kapanış konuşması (aşağıda, Bölüm 8).",
    tus="—"))

# ============== 3. GUCLU CUMLELER ==============
story.append(Paragraph("3. Güçlü Cümleler (sırasında düşür)", h2_style))
story.append(section_rule())
strong = [
    "Tasarım iterasyonlarını Unity editörü açıp kapatmadan, oyun çalışırken "
    "yapan bir oyun-içi araç yazdım.",
    "Otomatik testlerin yeşil olması yetmez — veri-güdümlü runtime doğrulama "
    "ile gerçek oynanışı kanıtladım.",
    "Geliştirme sürecini de bir mühendislik problemi olarak ele aldım: "
    "çok-ajanlı AI orkestrasyonu + sorgulanabilir kod-grafı.",
    "Mimari iddiamı graf-metrikleriyle destekledim: god-node’ların çoğunluğu "
    "tooling katmanı.",
]
for s in strong:
    box = Table([[Paragraph("● " + s, strong_style)]], colWidths=[CW])
    box.setStyle(TableStyle([
        ("LINEBEFORE", (0, 0), (0, -1), 3, TEAL),
        ("BACKGROUND", (0, 0), (-1, -1), TEAL_LIGHT),
        ("LEFTPADDING", (0, 0), (-1, -1), 8),
        ("RIGHTPADDING", (0, 0), (-1, -1), 8),
        ("TOPPADDING", (0, 0), (-1, -1), 5),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 5),
    ]))
    story.append(box)
    story.append(Spacer(1, 3))

# ============== 4. HOCAYA NOTLAR (tablo) ==============
story.append(Paragraph(
    "4. Hocaya Notlar — “bu var, şöyle güzelleşecek”", h2_style))
story.append(section_rule())
notes_rows = [
    ["Combat / Boss",
     "Çalışıyor. İyileşecek: düşman çeşitliliği + skill VFX."],
    ["Build Mode (F2)",
     "Oyun-içi seviye editörü. İyileşecek: Lights/Decals + oda kaydet/yükle."],
    ["Director Mode",
     "Canlı tuning. İyileşecek: kart tasarımı Hades-stili ikon + badge."],
    ["HUD",
     "Modern sol-alt. İyileşecek: HP rengi crimson, can-düşük efekti."],
    ["Silah",
     "8-yön mount + ön/arka. İyileşecek: yön ince-ayarı."],
    ["Elementalist",
     "İkinci sınıf var; eksik: 8-yön sprite + skill VFX (kredi). "
     "Uzun tutma."],
    ["ASIL GÜÇ",
     "Mimari + tooling + AI-süreç + graphify ile veriyle-kanıtlı tez."],
]
story.append(styled_table(["Sistem", "Söyle"], notes_rows,
                          [34 * mm, CW - 34 * mm], last_row_highlight=True))

# ============== 5. BILMEN GEREKENLER ==============
story.append(Paragraph("5. Bilmen Gerekenler (hızlı referans)", h2_style))
story.append(section_rule())
know = [
    "%s environment + vertical slice + reusable oyun-içi tooling. "
    "Eksen ~%%20 oyun / %%60 mimari / %%20 graphify." % b("Tez:"),
    "%s Unity 6, URP 2D, C#, ScriptableObject veri-güdümlü, Input System."
    % b("Teknoloji:"),
    "%s 6925 node / 118 community; god-node ~6/10 editor = tooling tezi "
    "sayısal kanıt." % b("Graphify:"),
    "%s Build Mode (F2) + Director Mode (runtime UI factory) + F1 debug."
    % b("Tooling:"),
    "%s 5/10 derinlemesine; demo ana sınıf Warblade." % b("Sınıflar:"),
    "%s çok-ajanlı AI (council + cx/Codex + ax/Gemini-Opus dispatch) + "
    "graphify." % b("Süreç:"),
    "%s combat-bug — “yeşil-assert ≠ çalışıyor” → data kök-neden "
    "(detectionRange) → cerrahi fix → full-flow doğrulama."
    % b("Vaka analizi:"),
]
story.append(bulleted(know))

# ============== 6. OLASI SORULAR ==============
story.append(Paragraph("6. Olası Sorular (sorulursa)", h2_style))
story.append(section_rule())
qa = [
    ("“Elementalist nerede / neden tek sınıf?”",
     "“Demo’da Warblade’e odaklandım; Elementalist’in sistemleri hazır ama "
     "8-yön sprite üretimi araç-kredi limitiyle beklemede — gösterirsem "
     "kısa tutarım.”"),
    ("“Oyun motoru mu yazdın?”",
     "“Hayır, Unity üzerinde; ama oyun-içi seviye editörü + runtime director "
     "aracı yazdım — projenin tooling/environment katmanı bu.”"),
    ("“AI ne kadarını yazdı?”",
     "“AI’ı çok-ajanlı bir mühendislik aracı olarak kullandım; mimariyi, "
     "kararları ve doğrulamayı ben kurdum; council + graphify ile denetledim.”"),
    ("“Test var mı?”",
     "“Evet + çok-katmanlı doğrulama: otomatik test → bağımsız AI-review → "
     "runtime-reproduce. Combat-bug vakası tam bunu gösteriyor.”"),
    ("“Neden roguelite / neden bu kapsam?”",
     "“Tek geliştiriciyle derinlik için: 5 sınıf derinlemesine + "
     "tekrar-oynanabilir döngü. Kapsamı araç/süreç yatırımıyla yönettim.”"),
    ("“Tamamlanmamış kısımlar?”",
     "Dürüst: 5/10 sınıf (tasarım kararı), Elementalist görselleri, bazı "
     "polish. “Her eksiği hızlandıran tool/süreç yazdım.”"),
    ("“Graphify nedir?”",
     "“Kodu sorgulanabilir bir bilgi-grafına çeviren araç; mimari iddiamı "
     "(tooling-ağırlıklı) god-node metrikleriyle kanıtlamamı sağladı.”"),
]
for q, a in qa:
    story.append(Paragraph('<font color="#21576B">►</font> ' + q, q_style))
    story.append(Paragraph(a, a_style))

# ============== 7. FALLBACK (amber kutu) ==============
story.append(Paragraph("7. Eğer Bir Şey Bozulursa", h2_style))
story.append(section_rule())
story.append(amber_box([
    ("Combat takılır / düşman idle:",
     "Sakin ol — “Bu canlı bir build; asıl gücü tooling” de, F2 Build Mode "
     "+ Director’a geç."),
    ("Oyun çöker:", "Yedek demo videosuna geç."),
    ("Boss’a ulaşamazsın:",
     "Telegraph’ı başka düşmanda / Director-spawn ile göster."),
    ("Genel:",
     "Asla panikleme; mimari / graphify / süreç anlatısı oynanıştan "
     "bağımsız güçlü — oraya kay."),
]))

# ============== 8. KAPANIS KONUSMASI ==============
story.append(Paragraph("8. Kapanış Konuşması (~30 sn)", h2_style))
story.append(section_rule())
story.append(speech_box(
    "Özetle: RIMA çalışan bir vertical slice — combat, boss, draft ve "
    "dallanan run-map. Ama asıl katkım oyun değil: oyun-içi seviye editörü, "
    "runtime director aracı, AI-destekli çok-ajanlı geliştirme sürecim ve "
    "bu mimari iddiayı graphify ile veriyle kanıtlamam. Sıradaki adımlar: "
    "sınıf görsellerinin tamamlanması ve polish. Dinlediğiniz için teşekkürler "
    "— sorularınızı alabilirim."))


# ---- Footer ----
def footer(canvas, doc):
    canvas.saveState()
    canvas.setFont(BASE_FONT, 7.5)
    canvas.setFillColor(MUTED)
    canvas.setStrokeColor(GRID)
    canvas.setLineWidth(0.4)
    canvas.line(18 * mm, 13 * mm, 192 * mm, 13 * mm)
    canvas.drawString(18 * mm, 9 * mm,
                      "RIMA — Rift Avcıları · Sunum Kılavuzu")
    canvas.drawRightString(192 * mm, 9 * mm,
                           "RIMA · Sunum Kılavuzu · S.%d" % doc.page)
    canvas.restoreState()


def build():
    out = os.path.join(HERE, "RIMA_Sunum_Kilavuzu.pdf")
    doc = SimpleDocTemplate(
        out, pagesize=A4,
        leftMargin=18 * mm, rightMargin=18 * mm,
        topMargin=15 * mm, bottomMargin=17 * mm,
        title="RIMA — Sunum Kilavuzu v2", author="Yasin Derya Bilgin",
    )
    doc.build(story, onFirstPage=footer, onLaterPages=footer)
    return out


if __name__ == "__main__":
    path = build()
    size = os.path.getsize(path)
    print("Font kullanildi: %s" % used_font)
    print("PDF: %s" % path)
    print("Boyut: %d bytes (%.1f KB)" % (size, size / 1024.0))
