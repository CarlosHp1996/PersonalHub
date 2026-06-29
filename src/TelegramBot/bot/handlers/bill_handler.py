# bot/handlers/bill_handler.py
import logging
from telegram import Update
from telegram.ext import ContextTypes
from services.api_client import api_client
from bot.formatters.bill_formatter import format_bills

logger = logging.getLogger(__name__)

async def list_bills(update: Update, context: ContextTypes.DEFAULT_TYPE):
    try:
        data = await api_client.get("/api/bills", params={"status": "pending"})
        message = format_bills(data)
    except Exception as e:
        logger.exception("Failed to list bills")
        message = f"❌ Não foi possível buscar as contas: {str(e)}"
    await update.message.reply_text(message, parse_mode="MarkdownV2")

async def add_bill(update: Update, context: ContextTypes.DEFAULT_TYPE):
    # Parse: /addbill <name> <amount> <due_date (YYYY-MM-DD)> [notes]
    args = context.args
    if not args or len(args) < 3:
        await update.message.reply_text(
            "Uso correto: `/addbill <nome> <valor> <data_vencimento (AAAA-MM-DD)> [observações]`\n\n"
            "Exemplo: `/addbill Internet 99.90 2026-07-10 Vivo`",
            parse_mode="Markdown"
        )
        return
    try:
        name = args[0]
        amount = float(args[1].replace(",", "."))
        due_date = args[2]
        notes = " ".join(args[3:]) if len(args) > 3 else None

        payload = {"name": name, "amount": amount, "dueDate": due_date, "notes": notes}
        result = await api_client.post("/api/bills", payload)
        
        await update.message.reply_text(
            f"✅ Conta *{result.get('name')}* de R$ {result.get('amount'):.2f} cadastrada com sucesso\\!",
            parse_mode="MarkdownV2"
        )
    except ValueError:
        await update.message.reply_text("❌ Valor inválido. Use um formato numérico como: `99.90`.")
    except Exception as e:
        logger.exception("Failed to add bill")
        await update.message.reply_text(f"❌ Erro ao cadastrar conta: {str(e)}")
