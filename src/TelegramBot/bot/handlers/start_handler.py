# bot/handlers/start_handler.py
from telegram import Update
from telegram.ext import ContextTypes

async def handle(update: Update, context: ContextTypes.DEFAULT_TYPE):
    message = (
        "👋 *Personal Hub*\\!\n\n"
        "Comandos disponíveis:\n"
        "• /bills \\- Listar contas pendentes\n"
        "• /addbill \\- Adicionar uma conta \\(Uso: `/addbill <nome> <valor> <AAAA-MM-DD> [obs]`\\)\n"
        "• /market \\- Resumo dos mercados\n"
        "• /polymarket \\- Portfólio Polymarket\n"
        "• /reminder \\- Agendar lembrete\n"
        "• /report \\- Gerar relatório matinal agora\n"
        "• /help \\- Exibir esta mensagem"
    )
    await update.message.reply_text(message, parse_mode="MarkdownV2")
