# Enterprise Decoupled AI Architecture Blueprint

This document defines the strict, decoupled structural architecture utilizing the **.NET Agent Framework**, **AG-UI Protocol**, and **Model Context Protocol (MCP)** across a multi-project .NET 11 layout.

## 🏗️ Architectural Topology & Reference Boundaries

The diagram below represents the complete project-level reference boundaries and bidirectional transaction pipes. 

The bidirectional lines (`▲` / `▼` / `◀` / `▶`) emphasize that while code references are strictly controlled to enforce a compilation firewall, **live application data, user requests, domain objects, and tool execution states flow back and forth seamlessly** across the allowed boundaries.

```
                      ┌────────────────────────────────────────────────────────┐
                      │  ClientManagement.Presentaion.Web (Blazor Server App)  │
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
        │        ClientManagement.MCP (MCP Server)               │ │
        │                                                        │ │
        │  Exposed MCP Tools Manifest (Flat Capability Line):    │ │
        │  ┌───────────────────────┐ ┌────────────────────────┐  │ │
        │  │   RAG Search Tool     │ │  Business Logic Tools  │  │ │
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

1. **`ClientManagement.Presentaion.Web` references `ClientManagement.AI` and `Business Logic Layer` ONLY.** It acts as the structural Composition Root shell. It handles regular application traffic, standard forms, and identity verification. It is explicitly blocked from referencing `ClientManagement.MCP`, ensuring presentation pages cannot bypass cognitive agents to invoke low-level tool handlers directly.
2. **`ClientManagement.AI` references `ClientManagement.MCP` ONLY.** The agent framework layer remains a pure cognitive manager. It has **zero runtime or compile-time visibility** into the database layouts, internal workflows, or algorithms of the Business Logic Layer.
3. **`ClientManagement.MCP` references the `Business Logic Layer` ONLY.** It serves as a secure, flat capabilities abstraction line. It hosts your **RAG Service** directly alongside transactional modules, exposing them horizontally to the AI layer as pure protocol tool configurations.

---

## 🔄 Bidirectional Data Flow Lifecycles

The system utilizes distinct bidirectional communication channels depending on whether an action is initiated by a traditional web layout or an intelligent agent:

### 1. Traditional/Direct Route (Web ⇄ Business Logic Layer)
*   **Web ➔ BLL:** When a user interacts with a standard, non-AI layout (e.g., loading a profile, filling out an explicit settings form, or viewing a static table), `ClientManagement.Presentaion.Web` bypasses the AI completely. It directly executes a scoped core service call down to the `Business Logic Layer`.
*   **BLL ➔ Web:** The business layer executes validation routines, reads from the database, and returns standard type-safe domain models or transaction receipts straight back up to the Blazor render tree to refresh the view immediately.

### 2. Agentic Route (Web ⇄ AI ⇄ MCP ⇄ BLL)
*   **Web ➔ AI:** A user submits an open-ended conversational request. The web app pipes it down into the backend `ClientManagement.AI` runtime over the real-time **AG-UI channel**.
*   **AI ➔ MCP:** The agent reasons over the request and realizes it needs specific tools. It submits a standardized JSON-RPC execution packet down to `ClientManagement.MCP`'s flat tool registry.
*   **MCP ➔ BLL:** The `Business Logic Tool` unpackages the parameters, satisfies local safety checks, and calls your scoped core domain engines or persistence models.
*   **BLL ➔ MCP:** The database validates mutations or fetches rows, returning success payloads back up to the awaiting `ClientManagement.MCP` interface wrapper.
*   **MCP ➔ AI:** `ClientManagement.MCP` (or the internal RAG search execution) flattens the result into clean text blocks and passes them back up to the `ClientManagement.AI` workspace.
*   **AI ➔ Web:** The Agent synthesizes an optimized response utilizing its newly acquired context, streaming incremental tokens and rich dynamic UI update components back up through **AG-UI** to update the user interface.
---

## 🛠️ Environment & Tooling Setup

To successfully build and run the application, the following tooling must be installed and configured on your development machine:

### 1. Development & Build Tools
- **.NET SDK**: (supporting .NET 10) Required for compiling the application and using the container publishing feature.
- **Docker Desktop** : Required to run the containerized versions of the Web, MCP, Ollama and QDrant projects.

### 2. Orchestration & Deployment Tools
- **Rancher Desktop**: Used as the local Kubernetes distribution (K3s) to run the application.
- **Helm**: The package manager for Kubernetes, used to manage the `izzy-bill` release.
- **kubectl**: The Kubernetes command-line tool (comes bundled with Rancher Desktop) to interact with the cluster.

### 3. Infrastructure Requirements
The Helm deployment automatically provisions the following as part of the application stack:
- **Qdrant**: A vector database for RAG (Retrieval-Augmented Generation).

---

## 🚀 Release to Rancher Desktop

To release the application to your local Rancher Desktop cluster, follow these steps:

### 1. Build the Containers
Before deploying, ensure you have built the container images for the Web and MCP projects using the .NET container publishing tool. These images should be available in your local Docker daemon (which Rancher Desktop utilizes).

### 2. Execute the Helm Deployment
Navigate to the Helm chart directory:
`cd ClientManagement.Presentation.Web/deployment`

Run the following command to install or upgrade the application:
```powershell
helm upgrade izzy-bill . -f .\values.yaml -f .\local-secrets.yaml --install --namespace development --create-namespace
```

### 3. Verify the Deployment
You can verify that the pods are running using `kubectl`:
```powershell
kubectl get pods -n development
```

### 4. Access the Application
Once the pods are in the `Running` state, you can access the application via the service endpoint defined in your `values.yaml` (typically via a NodePort or Ingress configured in the chart).

---

## ✅ Verifying a Successful Deployment

A successful deployment in Rancher Desktop should be verified through the dashboard or CLI. 

### The Multi-Container Pod Architecture
Unlike traditional deployments where one pod equals one container, the **Web project is deployed as a multi-container pod**. This means a single Pod instance encapsulates multiple interdependent services to ensure low-latency communication and shared lifecycle management.

In a successful deployment, you should observe the following:

1. **Pod Status**: The pod (e.g., `clientmanagement-presentation-web-...`) should show a status of `Running`.
2. **Container Readiness**: Within that single pod, you will see multiple containers running simultaneously:
   - **Web Container**: The Blazor Server application.
   - **MCP Container**: The Model Context Protocol server.
   - **Ollama Container**: The local LLM runtime.
3. **Supporting Services**: Separate pods for **Qdrant** should also be in the `Running` state to handle vector storage.

**Visual Confirmation:**
When viewing the deployment in the Rancher Desktop GUI, look for the pod icon that indicates multiple containers. Clicking into the pod details should reveal that all three core containers (Web, MCP, and Ollama) are healthy and have passed their readiness probes.

## 🔍 Production Verification Reference: Successful Deployment State

The following section defines the exact visual and technical characteristics of a verified, healthy **`izzy-bill`** release within the Rancher Desktop Kubernetes dashboard environment. Use this as a baseline reference during smoke testing and cluster audits.

### 1. High-Level Workloads Matrix
When navigating to the **Workloads** view, a fully operational deployment must satisfy the following state boundaries:

*   **Target Namespace**: All resources must belong exclusively to the **`development`** namespace.
*   **Compilation Firewall Validation**: 
    *   **`clientmanagement-presentation-web`**: Deployed cleanly as a **Deployment** type. This acts as the structural Composition Root shell.
    *   **`qdrant`**: Deployed cleanly as a **StatefulSet** type to handle the persistence layer for RAG vector storage.
*   **Health and Stability Metrics**:
    *   The **Health** column must display an unbroken, uniform **green status bar** for all workloads, indicating that all underlying replica counts and readiness probes match their desired configurations.
    *   The **Restarts** count across all top-level workloads must sit firmly at **`0`**, validating runtime memory stability and the absence of application boot loops.

### 2. Deployment Resource Specification & Scale
Clicking into the **`clientmanagement-presentation-web`** Deployment reveals the health status of the orchestrator:

*   **Active Status**: The core deployment resource must be explicitly flagged as **`Active`**.
*   **Pod Scale Metrics**: The cluster orchestration must reflect complete parity between desired and actual states:
    *   **`Ready: 1/1`** — confirming the scheduled pod has completed its lifecycle initialization.
    *   **`Up-to-date: 1`** — confirming the latest container image digests are pulled and applied.
    *   **`Available: 1`** — confirming the network endpoints are ready to accept traffic.
*   **Pods by State**: The dashboard visual block must show exactly **`1 Running`** pod instance.

### 3. Multi-Container Pod Anatomy
Because the Web project is purposefully deployed as a **multi-container pod** to ensure low-latency communication across the AG-UI and MCP Standard JSON-RPC channels, a status of `Running` is not enough. You must verify that all specialized application layers are healthy.

Opening the individual pod instance (e.g., `clientmanagement-presentation-web-[replica-hash]-[pod-id]`) and navigating to the **Containers** tab must show **`Ready: 3/3`** with three distinct sub-components running concurrently:

| Container Name | Runtime Role | Expected Readiness | Expected Restarts |
| :--- | :--- | :--- | :--- |
| **`clientmanagement-presentation-web`** | Blazor Server App / AG-UI Transport Shell | `✓` (Ready) | `0` |
| **`clientmanagement-mcp`** | Model Context Protocol Server (Exposed Tools Manifest) | `✓` (Ready) | `0` |
| **`ollama`** | Local LLM Runtime Engine | `✓` (Ready) | `0` |

*Note: Any restart count greater than 0 or a missing checkmark on any individual container layer indicates a partial pod failure (e.g., a tool handler crash or a localized LLM context error), even if the top-level Pod status claims to be "Running".*

