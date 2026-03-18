# Iron OCR Demo Repository Project Plan

This document outlines the development plan for building 5 demonstration projects within this repository for IronOCR. Each project is scheduled for approximately 2 weeks of development, starting October 1st, 2025.

## Repository Structure

### Folder Organization

**Iron OCR Code Storage:**
- Iron OCR code should be stored in its own class along with any core functionality being demonstrated
- This separation makes it easier for users to distinguish Iron code from utility code
- Each project subfolder should have its own utility class
- Shared utility code used by all projects should be placed in a `Utils` folder at the same level as the projects

**Documentation:**
- Documentation should be stored in separate files within dedicated documentation folders
- One `.md` file per project
- The root README should contain:
  - Shared setup instructions
  - Brief introduction to each project
  - Links to individual documentation folders

### Directory Structure

```
├── Data/
│   └── [Subfolders describing particular test data]
├── Code/
│   ├── [Feature folders with classes and methods]
│   ├── [Controller classes for CLI interaction]
│   └── Utilities/
├── Documentation/
│   └── [Individual project documentation files]
└── README.md
```

## Project Timeline and Deliverables

| Project/Demo | Features to Demonstrate | Delivery Date |
|--------------|------------------------|---------------|
| **Read PDF** | • Demonstrate data structure<br>• Add different languages | October 1, 2025 |
| **Filter Demonstration** | • Show impact of each filter | October 15, 2025 |
| **Reading IDs** | • Filter wizard<br>• Region of interest functionality | October 29, 2025 |
| **Advanced Scan** | • Read license plates<br>• Read passports | November 12, 2025 |
| **TBD** |  |  |

## Implementation Guidelines

Each project should follow these standards:
- Clean separation between Iron OCR functionality and utility code
- Comprehensive CLI controllers for easy demonstration
- Well-documented code with clear examples
- Test data organized in the Data folder with descriptive subfolders
- Individual documentation explaining setup, usage, and expected outcomes