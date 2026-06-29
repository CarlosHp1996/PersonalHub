import pytest
import httpx
from services.api_client import api_client
from config import settings

@pytest.mark.asyncio
async def test_get_calls_correct_url(mocker):
    # Mock httpx.AsyncClient.get
    mock_response = mocker.MagicMock(spec=httpx.Response)
    mock_response.json.return_value = {"data": [], "summary": {}}
    mock_response.raise_for_status = mocker.MagicMock()
    
    mock_get = mocker.patch("httpx.AsyncClient.get", return_value=mock_response)
    
    response = await api_client.get("/api/bills", params={"status": "pending"})
    
    mock_get.assert_called_once_with(
        f"{settings.API_BASE_URL}/api/bills",
        headers={"X-Internal-Key": settings.API_SECRET_KEY},
        params={"status": "pending"}
    )
    assert response == {"data": [], "summary": {}}

@pytest.mark.asyncio
async def test_get_raises_exception_on_error(mocker):
    # Mock httpx.AsyncClient.get to raise HTTPStatusError
    mock_response = mocker.MagicMock(spec=httpx.Response)
    mock_response.raise_for_status.side_effect = httpx.HTTPStatusError(
        message="Internal Server Error",
        request=mocker.MagicMock(spec=httpx.Request),
        response=mock_response
    )
    
    mocker.patch("httpx.AsyncClient.get", return_value=mock_response)
    
    with pytest.raises(httpx.HTTPStatusError):
        await api_client.get("/api/bills")

@pytest.mark.asyncio
async def test_post_sends_correct_body(mocker):
    mock_response = mocker.MagicMock(spec=httpx.Response)
    mock_response.json.return_value = {"id": "some-id", "name": "Internet"}
    mock_response.raise_for_status = mocker.MagicMock()
    
    mock_post = mocker.patch("httpx.AsyncClient.post", return_value=mock_response)
    
    payload = {"name": "Internet", "amount": 90.0, "dueDate": "2026-06-01"}
    response = await api_client.post("/api/bills", payload)
    
    mock_post.assert_called_once_with(
        f"{settings.API_BASE_URL}/api/bills",
        headers={"X-Internal-Key": settings.API_SECRET_KEY},
        json=payload
    )
    assert response == {"id": "some-id", "name": "Internet"}
