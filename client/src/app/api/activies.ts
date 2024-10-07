import {AddEntryInterface} from "../interface/Activity";

// This base url should be defined in a seperate file so that it can be imported to all apis
const BASE_URL = "http://localhost:3001/api";

export async function getAllActivities() {
    const response = await fetch(`${BASE_URL}/activities`);
    return response.json();
}

export async function getProjectEntries(projectId: string) {
    const response = await fetch(`${BASE_URL}/activityregister?projectId=${projectId}`);
    return response.json();
}

export async function addProjectActivity(postData: AddEntryInterface) {
    const response = await fetch(
        `${BASE_URL}/activityregister`,
        {
            method: 'POST',
            body: JSON.stringify(postData),
            headers: {
                'Content-Type': 'application/json'
            },
        }
    );

     const data = await response.json();

     // Return both status and data
     return {
         status: response.status,
         data: data
     };


}