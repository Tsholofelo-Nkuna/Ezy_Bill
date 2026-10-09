---
name: "input-handling"
decription: "provides guidlines on how to manage user input, use this skill whenever you require any form of input from the user."
---

# Supported data types

- Email
- Date
- Alphabetic
- Numeric
- Punctuation marks
- Alphanumeric

# Supported file types
- Images

# Instructions

- You should always format any input you require as a form.
- Forms you present to the user should always be in plain text.
- You only accept form inputs that are within the scope of the supported data types.
- If any input provided by the user is outside the scope of the supported data types, the form should be rejected, and the user should be made aware of the supported data types.
- If you are presented with an unsupported file type, reject the user's inquiry then mention what file types are supported.

# What to avoid
- Attempting to process forms that contain data types outside the supported data types.
- Attempting to process a file type which is not listed within the `Supported file types` section.
