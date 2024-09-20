# Test Technique - Damian GIL

## Description
Système de diffusion audio distribué avec **C#** et **RabbitMQ**. Le projet est composé de :
- **PosteOpérateur** : Enregistre ou charge un fichier audio et l'envoie au **ServeurCentral**.
- **ServeurCentral** : Diffuse l'audio reçu à toutes les instances d'**ÉlémentSonore**.
- **ÉlémentSonore** : Reçoit et joue l'audio diffusé par le serveur.

## Technologies
- **C#**, **RabbitMQ**, **Arecord**, **Aplay**, ***NAudio*** n'étant pas compatible Fedora/Linux
- **Fedora** utilisé pour le développement

## Prérequis
1. **RabbitMQ** :
   - Installer RabbitMQ : https://www.rabbitmq.com/download.html
   - Démarrer RabbitMQ : `sudo systemctl start rabbitmq-server`
2. **Arecord** et **Aplay** :
   - Installer : `sudo dnf install alsa-utils`
3. **.NET SDK** :
   - Installer : `sudo dnf install dotnet-sdk-7.0`

## Instructions
1. **Démarrer RabbitMQ** :
   - `sudo systemctl start rabbitmq-server`
2. **ServeurCentral** :
   - Dans le répertoire `ServeurCentral` : `dotnet run`
3. **ÉlémentSonore** :
   - Dans le répertoire `ElementSonore` : `dotnet run`
4. **PosteOpérateur** :
   - Dans le répertoire `PosteOperateur` : `dotnet run`

L'audio sera capté/chargé via **PosteOpérateur**, diffusé par **ServeurCentral** et joué sur les instances d'**ÉlémentSonore**.

## Contact
Pour toute question, n'hésitez pas à me contacter.
