import React, { useState, useEffect } from "react";
import { addProjectActivity, getAllActivities } from "../api/activies";
import { getAllProjects } from "../api/projects";
import ProjectInterface from "../interface/Project";
import {ActivityInterface, AddEntryInterface} from "../interface/Activity";
import NavigateButton from "../components/NavigateBack";

export default function AddEntryForm() {
    const [projects, setProjects] = useState<ProjectInterface[]>([]);
    const [activities, setActivities] = useState<ActivityInterface[]>([]);
    const [selectedProject, setSelectedProject] = useState("");
    const [selectedActivity, setSelectedActivity] = useState("");
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const [successMessage, setSuccessMessage] = useState("");

    // Fetch activities and Projects
    useEffect(() => {
        const fetchActivities = async () => {
            try {
                const data = await getAllActivities(); 
                setActivities(data);
            } catch (error) {
                console.error("Error fetching projects:", error);
            }
        };

        fetchActivities();

        const fetchProjects = async () => {
            try {
                const data = await getAllProjects(); 
                setProjects(data);
            } catch (error) {
                console.error("Error fetching projects:", error);
            }
        };
        fetchProjects();
    }, []); // Empty dependency array to only run once on component mount

    // Handle form submission
    const handleSubmit = (event: { preventDefault: () => void; }) => {
        event.preventDefault();
        const formData = {
            projectId: parseInt(selectedProject, 10),
            activityId: parseInt(selectedActivity, 10),
            startTime: startDate,
            endTime: endDate,
        };

        const addEntry = async (formData: AddEntryInterface) => {
            setErrorMessage("")
            setSuccessMessage("")
            try {
                const data = await addProjectActivity(formData); 
                if (data.status >= 200 && data.status < 300) {
                    setSuccessMessage("Activity registered successfully!");
                } else {
                    const errorMsg = JSON.stringify(data.data);
                        setErrorMessage(`Failed to register activity. Please enter a value for ${errorMsg}`);
                }
            } catch (error) {
                setErrorMessage("An unexpected error occurred. Please try again.");
                console.error("Networkerror fetching projects:", error);
            }
        };
        addEntry(formData); 
    };

    return (
        <div style={{ margin: '2rem 2rem 0rem 2rem' }}>
            {successMessage && (
                <div className="bg-green-100 text-green-800 border border-green-300 px-4 py-3 rounded mb-4">
                    {successMessage}
                </div>
            )}
            {errorMessage && (
                <div className="bg-red-100 text-red-800 border border-red-300 px-4 py-3 rounded mb-4">
                    {errorMessage}
                </div>
            )}
            <form onSubmit={handleSubmit} className="max-w-md mx-auto p-4 bg-white shadow-md rounded">
                <div className="mb-4">
                    <label htmlFor="project" className="block text-gray-700 font-bold mb-2">Project</label>
                    <select
                        id="project"
                        value={selectedProject}
                        onChange={(e) => setSelectedProject(e.target.value)}
                        className="block appearance-none w-full bg-white border border-gray-400 hover:border-gray-500 px-4 py-2 pr-8 rounded shadow leading-tight focus:outline-none focus:shadow-outline"
                    >
                        <option value="">Select Project</option>
                        {projects.map((project) => (
                            <option key={project.id} value={project.id}>
                                {project.name}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="mb-4">
                    <label htmlFor="activity" className="block text-gray-700 font-bold mb-2">Activity</label>
                    <select
                        id="activity"
                        value={selectedActivity}
                        onChange={(e) => setSelectedActivity(e.target.value)}
                        className="block appearance-none w-full bg-white border border-gray-400 hover:border-gray-500 px-4 py-2 pr-8 rounded shadow leading-tight focus:outline-none focus:shadow-outline"
                    >
                        <option value="">Select Activity</option>
                        {activities.map((activity) => (
                            <option key={activity.id} value={activity.id}>
                                {activity.activityName}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="mb-4">
                    <label htmlFor="startDate" className="block text-gray-700 font-bold mb-2">Start Date</label>
                    <input
                        type="datetime-local"
                        id="startDate"
                        value={startDate}
                        onChange={(e) => setStartDate(e.target.value)}
                        className="block w-full bg-white border border-gray-400 hover:border-gray-500 px-4 py-2 rounded shadow focus:outline-none focus:shadow-outline"
                    />
                </div>

                <div className="mb-4">
                    <label htmlFor="endDate" className="block text-gray-700 font-bold mb-2">End Date</label>
                    <input
                        type="datetime-local"
                        id="endDate"
                        value={endDate}
                        onChange={(e) => setEndDate(e.target.value)}
                        className="block w-full bg-white border border-gray-400 hover:border-gray-500 px-4 py-2 rounded shadow focus:outline-none focus:shadow-outline"
                    />
                </div>

                <div className="mb-4 flex space-x-4">
                    <button
                        type="submit"
                        className="flex-1 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
                    >
                        Submit
                    </button>

                    <NavigateButton route="/" label="Cancel" className="flex-1 bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 rounded"/>
                </div>

            </form>
        </div>

    );
}
