const BASE_URL = "http://localhost:3001/api";

export async function getAllProjects() {
    const response = await fetch(`${BASE_URL}/projects`);
    return response.json();
}

