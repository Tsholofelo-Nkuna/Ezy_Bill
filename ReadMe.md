# Enterprise Decoupled AI Architecture Blueprint

This document defines the strict, decoupled structural architecture utilizing the **.NET Agent Framework**, **AG-UI Protocol**, and **Model Context Protocol (MCP)** across a multi-project .NET 11 layout.

## 🏗️ Architectural Topology & Reference Boundaries

The diagram below represents the complete project-level reference boundaries and bidirectional transaction pipes. 

The bidirectional lines (`▲` / `▼` / `◀` / `▶`) emphasize that while code references are strictly controlled to enforce a compilation firewall, **live application data, user requests, domain objects, and tool execution states flow back and forth seamlessly** across the allowed boundaries.

```
                      ┌────────────────────────────────────────────────────────┐
                      │                  ClientManagement.Web                  │
                      │         (Composition Root Shell / AG-UI Transport)     │
                      └───────────────▲───┬────────────────────────▲───┬───────┘
                                      │   │                        │   
               (AG-UI Protocol Data)  │   │                        │   
                                      │   ▼                        │   
        ┌──────────────────────────────────────────────────┐       │
        │               ClientManagement.AI                │       │
        │             (Cognitive Brain Layer)              │       │ (Traditional Page Requests
        └─────────────────────▲───┬────────────────────────┘       │  & Core Domain Forms Data)
                              │   │                                │
     (MCP Standard JSON-RPC)  │   │                                │
                              │   ▼                                │
        ┌────────────────────────────────────────────────────────┐ │
        │                    ClientManagement.MCP                │ │
        │                                                        │ │
        │  Exposed MCP Tools Manifest (Flat Capability Line):    │ │
        │  ┌───────────────────────┐ ┌────────────────────────┐  │ │
        │  │   RAG Search Tool     │ │  Business Logic Tool   │  │ │
        │  │  (Queries Vector DB)  │ │ (Triggers Domain Core) │  │ │
        │  └───────────────────────┘ └────────────────────────┘  │ │
        └─────────────────────-───┬──────────────────────────────┘ │
                              │   │                                │
 (Internal Tool Capabilities) │   │                                │
                              │   ▼                                ▼
        ┌──────────────────────────────────────────────────────────────────────┐
        │                 ClientManagement.BusinessLogicLayer                  │
        │         (Core Domain, DB Context & Validation Rules)                 │
        └──────────────────────────────────────────────────────────────────────┘
```

---

## 🚫 Core Isolation Rules

1. **`Blazor.Web` references `Blazor.AI` and `Business Logic Layer` ONLY.** It acts as the structural Composition Root shell. It handles regular application traffic, standard forms, and identity verification. It is explicitly blocked from referencing `Blazor.MCP`, ensuring presentation pages cannot bypass cognitive agents to invoke low-level tool handlers directly.
2. **`Blazor.AI` references `Blazor.MCP` ONLY.** The agent framework layer remains a pure cognitive manager. It has **zero runtime or compile-time visibility** into the database layouts, internal workflows, or algorithms of the Business Logic Layer.
3. **`Blazor.MCP` references the `Business Logic Layer` ONLY.** It serves as a secure, flat capabilities abstraction line. It hosts your **RAG Service** directly alongside transactional modules, exposing them horizontally to the AI layer as pure protocol tool configurations.

---

## 🔄 Bidirectional Data Flow Lifecycles

The system utilizes distinct bidirectional communication channels depending on whether an action is initiated by a traditional web layout or an intelligent agent:

### 1. Traditional/Direct Route (Web ⇄ Business Logic Layer)
*   **Web ➔ BLL:** When a user interacts with a standard, non-AI layout (e.g., loading a profile, filling out an explicit settings form, or viewing a static table), `Blazor.Web` bypasses the AI completely. It directly executes a scoped core service call down to the `Business Logic Layer`.
*   **BLL ➔ Web:** The business layer executes validation routines, reads from the database, and returns standard type-safe domain models or transaction receipts straight back up to the Blazor render tree to refresh the view immediately.

### 2. Agentic Route (Web ⇄ AI ⇄ MCP ⇄ BLL)
*   **Web ➔ AI:** A user submits an open-ended conversational request. The web app pipes it down into the backend `Blazor.AI` runtime over the real-time **AG-UI channel**.
*   **AI ➔ MCP:** The agent reasons over the request and realizes it needs specific tools. It submits a standardized JSON-RPC execution packet down to `Blazor.MCP`'s flat tool registry.
*   **MCP ➔ BLL:** The `Business Logic Tool` unpackages the parameters, satisfies local safety checks, and calls your scoped core domain engines or persistence models.
*   **BLL ➔ MCP:** The database validates mutations or fetches rows, returning success payloads back up to the awaiting `Blazor.MCP` interface wrapper.
*   **MCP ➔ AI:** `Blazor.MCP` (or the internal RAG search execution) flattens the result into clean text blocks and passes them back up to the `Blazor.AI` workspace.
*   **AI ➔ Web:** The Agent synthesizes an optimized response utilizing its newly acquired context, streaming incremental tokens and rich dynamic UI update components back up through **AG-UI** to update the user interface.
