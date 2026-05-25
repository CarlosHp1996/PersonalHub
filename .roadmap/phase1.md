# Phase 1: Repository & Infrastructure Base

Establish the foundation for the monorepo, configure local environment variables, create the dockerized infrastructure (PostgreSQL, Redis, and RabbitMQ), and verify network connectivity and healthy service statuses.

- **Goal**: Monorepo created, Docker Compose running all infrastructure services, `.env` configured.
- **Estimated Time**: 3–4 hours
- **Branch**: `chore/project-setup` (Or atomic feature branches: `chore/folders-setup`, `chore/env-setup`, `chore/infra-setup`)

---

## Task Breakdown

### 📋 Task 1.1: Monorepo Directory Structure & Gitignore Setup
Create the physical folder structure for the monorepo and protect credentials/runtime outputs by refining the `.gitignore` rules.
- **Folder Scope**: Workspace Root
- **Branch**: `chore/folders-setup`
- **Steps**:
  1. Create the top-level monorepo directories:
     - `src/AssistantAPI` (.NET 8 Microservice)
     - `src/TelegramBot` (Python Microservice)
     - `infra/nginx` (Nginx configuration)
     - `infra/scripts` (Deployment and backup scripts)
     - `.github/workflows` (CI/CD GitHub Actions pipelines)
  2. Create/edit `.gitignore` at the repository root to block `.env`, credentials, visual editor configurations, and compiler output folders:
     - Add `.env`
     - Add `.NET` build artifacts: `bin/`, `obj/`
     - Add Python caching and runtime: `__pycache__/`, `*.pyc`, `.venv/`
     - Add IDE specific files: `.idea/`, `.vscode/`, `*.user`
- **Definition of Done (DoD)**:
  - Folders successfully created.
  - `.gitignore` configured to ensure untracked local credentials (`.env`) or build logs are not tracked by git.
  - Atomic commit (`chore(setup): establish monorepo structure and gitignore`) pushed and Pull Request opened.

---

### 📋 Task 1.2: Environment Variables Template Configuration
Configure the model configurations and local settings templates for all database connections and third-party APIs.
- **Folder Scope**: Workspace Root
- **Branch**: `chore/env-setup`
- **Steps**:
  1. Create `.env.example` at the repository root with placeholders for all environment variables from ARCHITECTURE.md:
     - **Postgres**: Database name, user, password, and C# connection string.
     - **Redis**: Connection endpoint (`redis:6379`).
     - **RabbitMQ**: Host name, user, and password credentials.
     - **Telegram**: Telegram Bot Token (from BotFather) and Target Chat ID.
     - **External APIs**: Binance URL, AwesomeAPI URL, Alpha Vantage Key, Open-Meteo URL, OpenWeatherMap Key, GNews Key, NewsAPI Key, CoinGecko URL.
     - **Polymarket**: CLOB Key, Secret, Passphrase, Wallet, WSS URL, CLOB REST URL.
     - **.NET API**: Base URL and internal Secret API Key for Bot authentication.
  2. Create a local `.env` file by copying the template: `cp .env.example .env` and fill it with secure dummy/development credentials.
- **Definition of Done (DoD)**:
  - `.env.example` successfully created and committed.
  - Local `.env` successfully configured and verified as untracked by Git (verifying that the `.gitignore` rule is working).
  - Atomic commit (`chore(env): create environment variables template`) pushed and Pull Request opened.

---

### 📋 Task 1.3: Docker Compose Infrastructure & Verification
Compose a highly resilient docker infrastructure orchestration file for all main databases and brokers, start them locally, and verify execution and database connections.
- **Folder Scope**: Workspace Root
- **Branch**: `chore/infra-setup`
- **Steps**:
  1. Create `docker-compose.yml` at the repository root.
  2. Configure the following 3 core infrastructure services:
     - **PostgreSQL 16 (Alpine)**: Database name, credentials, persistent volume, and health check validation utilizing `pg_isready`.
     - **Redis 7 (Alpine)**: Local caching data persistent volume and `redis-cli ping` health check.
     - **RabbitMQ 3 (Management-Alpine)**: Messaging credentials, persistent volume, internal network setup, and container status health check diagnostics.
  3. Spin up infrastructure services locally using: `docker compose up -d`
  4. Verify that all 3 services show a `healthy` status via: `docker compose ps`
  5. Test database and message broker connectivity by running diagnostic commands within the active containers.
- **Definition of Done (DoD)**:
  - `docker-compose.yml` successfully configured.
  - All 3 containers show a `healthy` status.
  - SQL, Cache, and Messaging connectivity successfully tested without errors.
  - Atomic commit (`chore(infra): setup docker-compose for database and message brokers`) pushed and Pull Request opened.

---

## Validation Checklist

- [ ] Folder structure matches ARCHITECTURE.md Section 3.
- [ ] `.env.example` committed, `.env` is in `.gitignore` and never tracked.
- [ ] `docker compose up -d` starts Postgres, Redis, and RabbitMQ without errors.
- [ ] All 3 containers show healthy status in `docker compose ps`.
- [ ] RabbitMQ management UI accessible at `http://localhost:15672` in local browser.
- [ ] Database connectivity verified locally.
