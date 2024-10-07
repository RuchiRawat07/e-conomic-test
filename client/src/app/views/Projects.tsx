import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Table from "../components/Table";
import { getAllProjects } from "../api/projects";
import ProjectInterface from "../interface/Project";

export default function Projects() {
    const navigate = useNavigate();
    const [searchQuery, setSearchQuery] = useState("");
    const [projects, setProjects] = useState<ProjectInterface[]>([]);
    const [filteredProjects, setFilteredProjects] = useState<ProjectInterface[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await getAllProjects();
                setFilteredProjects(data);
                setProjects(data);
            } catch (error) {
                console.error("Error fetching projects:", error);
            }
        };

        fetchData(); 
    }, []); 

      useEffect(() => {
        if (searchQuery) {
            setFilteredProjects(
                projects.filter((project) =>
                    project.name.toLowerCase().includes(searchQuery.toLowerCase())
                )
            );
        } else {
            setFilteredProjects(projects);
        }
    }, [searchQuery]);

    const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSearchQuery(event.target.value);
    };

    const handleAddEntryClick = () => {
        navigate("/add-entry");
    };

    return (
        <>
            <div className="flex items-center my-6">
                <div className="w-1/2">
                    <button 
                    onClick={handleAddEntryClick}
                    className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
                        Add entry
                    </button>
                </div>

                <div className="w-1/2 flex justify-end">
                    <form>
                        <input
                            className="border rounded-full py-2 px-4"
                            type="search"
                            placeholder="Search Projects"
                            aria-label="Search"
                            value={searchQuery}
                            onChange={handleSearchChange}
                        />
                        
                    </form>
                </div>
            </div>

            <Table projects={filteredProjects}/>
        </>
    );
}
