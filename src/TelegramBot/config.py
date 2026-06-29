from pydantic_settings import BaseSettings, SettingsConfigDict

class Settings(BaseSettings):
    model_config = SettingsConfigDict(env_file=".env", env_file_encoding="utf-8", extra="ignore")

    TELEGRAM_BOT_TOKEN: str
    TELEGRAM_CHAT_ID: int
    API_BASE_URL: str = "http://localhost:5000"
    API_SECRET_KEY: str = "temporary_secret_key"

    RABBITMQ_HOST: str = "localhost"
    RABBITMQ_USER: str = "guest"
    RABBITMQ_PASSWORD: str = "guest"

    POLYMARKET_API_KEY: str = ""
    POLYMARKET_API_SECRET: str = ""
    POLYMARKET_API_PASSPHRASE: str = ""
    POLYMARKET_WALLET_ADDRESS: str = ""
    POLYMARKET_WSS_URL: str = "wss://ws-subscriptions-clob.polymarket.com/ws/user"
    POLYMARKET_CLOB_API_URL: str = "https://clob.polymarket.com"

    GNEWS_API_KEY: str = ""
    NEWSAPI_KEY: str = ""

settings = Settings()
