# MailSending

Miscellaneous email sending (SMTP) tests for further integration.

## Docker

To launch the Docker Compose, execute the following commands:

```sh
docker compose build
docker compose up -d
```

## Environment

Execute the following steps to configure your environment before executing this project:

1. Launch the Docker Compose file.
2. Copy the contents of the `secrets.example.json` file.
3. Right-click the `MailSending` project, then click `Manage User Secrets`.
4. Paste the contents inside this file and replace the placeholders with your settings.

You can disable a mail sending method by adding an underscore before its name (for example, `Gmail` becomes `_Gmail`).

## MailCatcher

Catches mail and serves it through a dream. MailCatcher runs a super simple SMTP server which catches any message sent to it to display in a web interface. Run mailcatcher, set your favourite app to deliver to smtp://127.0.0.1:1025 instead of your default SMTP server, then check out http://127.0.0.1:1080 to see the mail that's arrived so far.
