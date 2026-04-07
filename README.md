

[cite_start]**Chanel CMS** is a high-end desktop Content Management System developed as part of the "Usability Engineering in Infrastructure Systems" course[cite: 1]. The application is designed to manage and showcase the most significant historical moments of the House of Chanel, from the opening of the first boutique to modern-day runway spectacles.

## 📸 Preview
Custom-designed interface featuring a minimalist luxury aesthetic with gold accents and a signature Coco Chanel watermark.*

## ✨ Features

### 👤 User Roles & Access Control
- **Admin:** Full control over content. [cite_start]Capability to add, edit, and delete iconic moments[cite: 7].
- [cite_start]**Visitor:** Read-only access to browse the history and view detailed information[cite: 8].
- [cite_start]All user data and sessions are securely handled via **XML serialization**[cite: 11, 43].

### 📝 Advanced Content Management
- [cite_start]**Table View:** Integrated `DataGrid` with custom templates for images, hyperlinks, and multi-selection via CheckBoxes[cite: 15, 16, 61].
- **Rich Text Editor:** A fully-featured text editor for descriptions, supporting:
  - [cite_start]Bold, Italic, Underline[cite: 27, 30].
  - [cite_start]Font family preview and size adjustment[cite: 27, 29].
  - [cite_start]Custom color picker with all system colors[cite: 33].
  - [cite_start]Real-time word count in the status bar[cite: 28].
- [cite_start]**Automated Logging:** Each entry automatically records the timestamp of its creation[cite: 34].

### 🎨 Design & Usability
- [cite_start]**Thematic Consistency:** A dark, elegant "Lux" theme using a palette of Gold (#BFA181) and Charcoal (#1a1a1a)[cite: 46, 47, 52].
- [cite_start]**Validation:** Robust input validation for all fields (including RichTextBox) with clear, field-specific error messages[cite: 54, 55, 56].
- [cite_start]**Interactive UX:** Responsive cursors, detailed ToolTips, and confirmation dialogs for critical actions (like deletion)[cite: 57, 60].

## 🛠️ Technical Stack
- **Framework:** .NET / WPF (Windows Presentation Foundation)
- [cite_start]**Language:** C# (Clean Code & naming conventions) [cite: 64, 65]
- [cite_start]**Data Storage:** - `*.xml` for object data and user accounts[cite: 11, 14].
  - [cite_start]`*.rtf` for styled textual content[cite: 14, 62].
- [cite_start]**Architecture:** MVVM-inspired structure with custom Converters and relative paths for portability[cite: 12, 63].

## 🚀 Getting Started
1. Clone the repository.
2. Open the `.sln` file in Visual Studio.
3. Ensure all image resources in `/Resources/` are set to `Build Action: Resource`.
4. Build and Run.

**Login Credentials:**
- *Admin:* `admin` / `admin123`
- *Visitor:* `visitor` / `visitor123`

---
[cite_start]Developed for the **Applied Software Engineering** department at FTN[cite: 1, 25, 50].
