# services/api_client.py
import httpx
from config import settings

class AssistantApiClient:
    def __init__(self):
        self._base_url = settings.API_BASE_URL
        self._headers = {"X-Internal-Key": settings.API_SECRET_KEY}

    async def get(self, path: str, params: dict = None) -> dict:
        async with httpx.AsyncClient(timeout=10.0) as client:
            response = await client.get(
                f"{self._base_url}{path}",
                headers=self._headers,
                params=params
            )
            response.raise_for_status()
            return response.json()

    async def post(self, path: str, body: dict) -> dict:
        async with httpx.AsyncClient(timeout=10.0) as client:
            response = await client.post(
                f"{self._base_url}{path}",
                headers=self._headers,
                json=body
            )
            response.raise_for_status()
            return response.json()

    async def patch(self, path: str) -> dict:
        async with httpx.AsyncClient(timeout=10.0) as client:
            response = await client.patch(
                f"{self._base_url}{path}",
                headers=self._headers
            )
            response.raise_for_status()
            return response.json()

api_client = AssistantApiClient()
