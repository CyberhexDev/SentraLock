# SentraLock – File Encryption & Decryption Tool

## Overview

**SentraLock** is a C# security application that allows users to encrypt and decrypt files to protect sensitive data from unauthorized access.

The program provides a simple interface for securing files using cryptographic techniques. A user can select a file, apply encryption with a key, and later restore the original file through the decryption process.

The goal of this project was to understand how modern applications protect stored data and to implement core cybersecurity concepts including confidentiality, secure storage, and controlled access.

---

## Purpose

Sensitive files stored on a computer can be accessed, copied, or stolen if they are not protected.
SentraLock prevents this by converting readable file data into encrypted data that cannot be opened without the proper key.

This project demonstrates how encryption is used in real-world applications such as:

* secure documents
* password managers
* protected backups
* confidential business records

---

## Features

* Encrypt any file type
* Decrypt previously encrypted files
* Key-based protection
* Prevents unauthorized file viewing
* Secure file transformation
* Simple user workflow

---

## Technologies Used

* **Language:** C#
* **Framework:** .NET
* **Application Type:** Desktop Application
* **Concepts Implemented:**

  * Cryptography fundamentals
  * File stream processing
  * Byte manipulation
  * Secure data handling
  * User input handling

---

## How It Works

### Encryption

1. The user selects a file.
2. The program reads the file as binary data.
3. The data is transformed using an encryption algorithm and a key.
4. The encrypted data is written to a new file.
5. The original content becomes unreadable without decryption.

### Decryption

1. The user selects the encrypted file.
2. The user provides the correct key.
3. The program reverses the encryption algorithm.
4. The original file contents are restored.

---

## Workflow

Plain File → Encryption Algorithm → Encrypted File
Encrypted File → Decryption Algorithm + Key → Original File

---

## Security Concepts Demonstrated

* Data confidentiality
* Key-based access
* Secure storage
* Reversible cryptographic operations
* Protection against unauthorized file access

---

## Example Use Case

A user wants to store personal or sensitive documents on a shared computer or USB drive.
They encrypt the file using SentraLock. Anyone who opens the file without the correct key will only see unreadable data.

---

## What This Project Demonstrates

* Secure software design
* File I/O operations in C#
* Binary data processing
* Implementation of encryption/decryption workflows
* Practical cybersecurity concepts

---

## Future Improvements

* Password strength requirements
* AES-256 encryption
* Drag-and-drop file support
* Folder encryption
* Hash verification (integrity check)
* GUI improvements

---

## Author

**Christopher Cummings**

---

## How to Run

1. Clone the repository

```
git clone https://github.com/CyberhexDev/SentraLock.git
```

2. Open the solution

* Open `SentraLock.sln` in Visual Studio

3. Build the project

```
Build → Build Solution
```

4. Run the application

```
Press F5 in Visual Studio
```

---

## License

This project is released under the MIT License.
