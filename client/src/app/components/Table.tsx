import React, { useEffect, useState } from "react";
import ProjectInterface from "../interface/Project";
import { useNavigate } from "react-router-dom";

export default function Table({ projects }: { projects: ProjectInterface[] }) {
    const [filteredProjects, setProjects] = useState<ProjectInterface[]>([]);
    const [sortOrder, setSortOrder] = useState<"asc" | "desc">("asc");

   
    const navigate = useNavigate();
    const handleProjectClick = (projectId : number) => {
        navigate(`/show-activities/${projectId}`);
    };

    // Empty dependency array to only run once on component mount
    useEffect(() => {
        setProjects(projects);
    }, [projects]); 

    // Formate the date to dd-mm-yyyy
    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        return `${day}-${month}-${year}`;
    };

    // Function to handle sorting of projects by deadline
    const handleSortByDeadline = () => {
        // Copy the projects array
        const sortedProjects = [...filteredProjects]; 

        sortedProjects.sort((a, b) => {
            const dateA = new Date(a.deadLine).getTime();
            const dateB = new Date(b.deadLine).getTime();
            return sortOrder === "asc" ? dateA - dateB : dateB - dateA;
        });

        // Update the projects array with sorted data
        // Toggle sort order
        setProjects(sortedProjects); 
        setSortOrder(sortOrder === "asc" ? "desc" : "asc"); 
    };

    return (
        <table className="table-fixed w-full">
            <thead className="bg-gray-200">
                <tr>
                    <th className="border px-4 py-2 w-12  text-center">#</th>
                    <th className="border px-4 py-2  text-center">Project Name</th>
                    <th className="border px-4 py-2  text-center">Start Date</th>
                    <th 
                        className="border px-4 py-2 cursor-pointer hover:bg-gray-300 text-center"
                        onClick={handleSortByDeadline}>
                        Deadline {sortOrder === "asc" ? "▲" : "▼"}  {/* Show sorting direction */}
                    </th>
                </tr>
            </thead>
            <tbody>
                {filteredProjects.length > 0 ? (
                    filteredProjects.map((project, index) => (
                        <tr key={index}>
                            <td className="border px-4 py-2 w-12">{index + 1}</td>
                            <td onClick={() => handleProjectClick(project.id)} className=" border px-4 py-2 cursor-pointer hover:bg-blue-100 hover:text-blue-600 transition-colors duration-200  text-center">{project.name}</td>
                            <td className="border px-4 py-2  text-center">
                                {formatDate(project.startDate)}
                            </td>
                            <td className="border px-4 py-2  text-center">
                                {formatDate(project.deadLine)}
                            </td>
                        </tr>
                    ))
                ) : (
                    <tr>
                        <td className="border px-4 py-2 text-center">
                            No projects available
                        </td>
                    </tr>
                )}
            </tbody>
        </table>
    );
}
