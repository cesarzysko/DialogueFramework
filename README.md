# DialogueFramework

A Dialogue Framework written in C# with reusability and flexibility as core design principles.

This project is built as a programming skill–polishing exercise, focusing on clean architecture, separation of concerns, and engine-agnostic design. It currently includes a Text Adventure sample implementation and is structured for future expansion.

---

### Goals

- Promote clean, maintainable architecture
- Ensure high reusability across projects
- Remain engine-agnostic
- Support multiple frontends (Console, Unity, MonoGame)

---

### Architecture Overview

- **Core:**
  - Nodes
  - Choices
  - Conditions
  - Actions
  - Dialogue Graph
  - Dialogue Runner

- **Frontend Layer**
  - Console (Text Adventure sample)
  - Unity (planned)
  - MonoGame (planned)

---

### Roadmap

- Dialogue framework core - **Done**
- Dialogue graph serialization support - **In Progress**
- Save / Load runtime state
- Dialogue editing
- Unity adapter layer
- Unity visual graph representation and editing
- MonoGame adapter layer