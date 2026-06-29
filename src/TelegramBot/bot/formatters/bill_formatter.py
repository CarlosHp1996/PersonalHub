# bot/formatters/bill_formatter.py
from datetime import datetime

def format_bills(data: dict) -> str:
    """
    Formats the response from GET /api/bills into a readable Telegram message.
    The response has:
    {
       "data": [ { id, name, amount, dueDate, status, paidAt, notes, isOverdue, daysUntilDue } ],
       "summary": { totalPending, totalOverdue, totalAmountDue }
    }
    """
    bills_list = data.get("data", [])
    summary = data.get("summary", {})

    if not bills_list:
        return "📅 *Contas*\n\nNão há nenhuma conta cadastrada ou pendente\\."

    lines = ["📅 *Contas Cadastradas:*"]
    
    # Sort bills: overdue first, then by due date
    sorted_bills = sorted(
        bills_list, 
        key=lambda b: (b.get("status") != "Overdue", b.get("dueDate", ""))
    )

    for bill in sorted_bills:
        name = escape_markdown(bill.get("name", "Sem Nome"))
        amount = f"{bill.get('amount', 0.0):,.2f}".replace(",", "X").replace(".", ",").replace("X", ".")
        due_date_str = bill.get("dueDate", "")
        status = bill.get("status", "Pending")
        notes = bill.get("notes")

        # Parse date to prettier format DD/MM/YYYY
        try:
            dt = datetime.strptime(due_date_str, "%Y-%m-%d")
            due_formatted = dt.strftime("%d/%m/%Y")
        except ValueError:
            due_formatted = escape_markdown(due_date_str)

        # Determine Emoji and status text
        if status == "Overdue":
            emoji = "⚠️"
            status_text = "Atrasada"
        elif status == "Paid":
            emoji = "✅"
            status_text = "Paga"
        else:
            emoji = "📅"
            status_text = "Pendente"

        line = f"{emoji} *{name}*: R$ {amount} \\(Venc: {due_formatted} \\- {status_text}\\)"
        if notes:
            line += f"\n   _Obs: {escape_markdown(notes)}_"
        lines.append(line)

    # Append summary
    total_pending = summary.get("totalPending", 0)
    total_overdue = summary.get("totalOverdue", 0)
    total_amount = f"{summary.get('totalAmountDue', 0.0):,.2f}".replace(",", "X").replace(".", ",").replace("X", ".")

    lines.append("\n📊 *Resumo:*")
    lines.append(f"• Pendentes: {total_pending}")
    lines.append(f"• Atrasadas: {total_overdue}")
    lines.append(f"• Total a Pagar: *R$ {total_amount}*")

    return "\n".join(lines)

def escape_markdown(text: str) -> str:
    """Escapes markdown special characters for Telegram MarkdownV2"""
    escape_chars = r"_*[]()~`>#+-=|{}.!"
    return "".join(f"\\{c}" if c in escape_chars else c for c in text)
