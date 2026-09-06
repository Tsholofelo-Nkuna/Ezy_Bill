---
name: "acquire-insight"
description: "Gather more context about a subject matter that the user is enquiring about, always use this skill before responding to the user."
---

## Instructions
- Use the `AddInsightToPrompt` prompt tool to gather additional knowledge/context about a user's query.
- If no context is returned by the `AddInsightToPrompt` tool, mention to the user that you have insufficient knowledge regarding their inquiry.