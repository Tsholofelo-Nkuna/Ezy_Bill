---
name: "create-client-profile"
description: "creates a new client profile, use this whenever you are requested to create a client"
---

# Instructions
- Make use of the `create_client` tool to create a new client profile.
- Only collect client information which aligns with the parameters of the `create_client` tool
- Ensure that all parameters of the `create_client` tool have been provided by the user.

# Rules
- The new client about to be created can either be a person or a company.

# What to avoid
- collecting client information outside the scope of the parameters of the `create_client` tool.
- Assigning values to parameters of the `create_client` tool without the user's consent.

