# main.py
import logging
import sys
from telegram.ext import ApplicationBuilder, CommandHandler
from bot.handlers import bill_handler, start_handler
from config import settings

logging.basicConfig(
    format="%(asctime)s - %(name)s - %(levelname)s - %(message)s",
    level=logging.INFO,
    handlers=[
        logging.StreamHandler(sys.stdout)
    ]
)
logger = logging.getLogger(__name__)

def main():
    logger.info("Initializing Telegram Bot...")
    
    app = ApplicationBuilder().token(settings.TELEGRAM_BOT_TOKEN).build()

    # Register handlers
    app.add_handler(CommandHandler("start", start_handler.handle))
    app.add_handler(CommandHandler("help", start_handler.handle))
    app.add_handler(CommandHandler("bills", bill_handler.list_bills))
    app.add_handler(CommandHandler("addbill", bill_handler.add_bill))

    logger.info("Telegram Bot started. Polling...")
    # run_polling() is a synchronous blocking method that manages the asyncio loop internally
    app.run_polling()

if __name__ == "__main__":
    try:
        main()
    except (KeyboardInterrupt, SystemExit):
        logger.info("Bot stopped.")
