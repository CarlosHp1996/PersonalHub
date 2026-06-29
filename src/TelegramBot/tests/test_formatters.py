import pytest
from bot.formatters.bill_formatter import format_bills

def test_format_bills_empty():
    data = {"data": [], "summary": {}}
    result = format_bills(data)
    assert "Não há nenhuma conta cadastrada ou pendente" in result

def test_format_bills_with_data():
    data = {
        "data": [
            {
                "id": "1",
                "name": "Light Bill",
                "amount": 150.50,
                "dueDate": "2026-07-10",
                "status": "Pending",
                "notes": "Please pay"
            },
            {
                "id": "2",
                "name": "Expired internet",
                "amount": 99.90,
                "dueDate": "2026-06-01",
                "status": "Overdue",
                "notes": None
            }
        ],
        "summary": {
            "totalPending": 2,
            "totalOverdue": 1,
            "totalAmountDue": 250.40
        }
    }
    
    result = format_bills(data)
    
    # Check escaping and formatting
    assert "⚠️" in result
    assert "Expired internet" in result
    assert "150,50" in result
    assert "99,90" in result
    assert "Total a Pagar" in result
    assert "250,40" in result
