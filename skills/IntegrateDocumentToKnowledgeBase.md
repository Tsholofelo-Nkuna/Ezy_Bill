# Skill: IntegrateDocumentToKnowledgeBase

## Description
This skill handles the end-to-end process of registering a physical file in the SQL database and indexing its content into the Qdrant Vector Store for a specific AI agent.

## ?? Required Parameters
| Parameter | Type | Description | Example |
| :--- | :--- | :--- | :--- |
| ilePath | String | The absolute path to the file on the local machine. | "C:\Docs\TaxLaw.pdf" |
| gentName | String | The name of the AI agent the file should be associated with. | "Jimmy" |
| ectorStoreName | String | The name of the Qdrant collection. | "Accounting" |
| piHost | String | The current running URL of the Web project. | "http://localhost:5068" |

## ?? Execution Workflow

### Step 1: SQL Registration
- **Target**: swyrbgd_izybill database -> AppFiles table.
- **Operation**: Ensure gentName column exists; UPSERT record with FileName, FileSize, MimeType, AgentName, and Contents (Binary).

### Step 2: DTO Construction
- Create a List<AppFileDto>.
- Convert file bytes to **Base64 string** for the Contents property.
- Use PascalCase property names for .NET compatibility.

### Step 3: Trigger Application UpSert
- **Endpoint**: {apiHost}/api/AppFile/{vectorStoreName}.
- **Method**: POST.
- **Payload**: JSON List<AppFileDto>.
- **Result**: Triggers server-side VectorStore.UpSert -> DocumentToolKit.ReadPdf -> EmbeddingGenerator -> Qdrant Upsert.

## ?? Invocation Example
"Use the **IntegrateDocumentToKnowledgeBase** skill for [filePath] for agent [agentName] using collection [vectorStoreName] at [apiHost]."
