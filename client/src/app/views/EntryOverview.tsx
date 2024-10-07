import React, { useState, useEffect } from "react"; 
import { getProjectEntries } from "../api/activies";
import { useParams } from "react-router-dom";
import { EntryInterface } from "../interface/Entry";
import NavigateButton from "../components/NavigateBack";


export default function EntryOverview() {
    const { projectId } = useParams<{ projectId: string }>(); 
    const [entries, setProjectentry] = useState<EntryInterface[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await getProjectEntries(projectId || '0'); 
                setProjectentry(data);
            } catch (error) {
                console.error("Error fetching entries:", error);
            }
        };

        fetchData();
    }, []);

        const formatDateTime = (dateString: string) => {
        const date = new Date(dateString);
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');
        return `${day}-${month}-${year} ${hours}:${minutes}`;
    };
    return (
        <>
            <div style={{ margin: '2rem 2rem 0rem 2rem' }}>
            <div className="overflow-x-auto mb-8">
                <table className="table-fixed w-full border-collapse border-spacing-0">
                    <thead className="bg-gray-200">
                        <tr>
                            <th className="border px-4 py-2 w-12">#</th>
                            <th className="border px-4 py-2">Project Name</th>
                            <th className="border px-4 py-2">Activity Type</th>
                            <th className="border px-4 py-2">Is Billable</th>
                            <th className="border px-4 py-2">Start Time</th>
                            <th className="border px-4 py-2">End Time</th>
                        </tr>
                    </thead>
                    <tbody>
                        {entries.length > 0 ? (
                            entries.map((entry, index) => (
                                <tr key={index}>
                                    <td className="border px-4 py-2 w-12">{index + 1}</td>
                                    <td className="border px-4 py-2  text-center">{entry.projectName}</td>
                                    <td className="border px-4 py-2  text-center">{entry.activityName}</td>
                                    <td className="border px-4 py-2  text-center">{entry.isBillable ? "Yes" : "No"}</td>
                                    <td className="border px-4 py-2  text-center">{formatDateTime(entry.startTime)}</td>
                                    <td className="border px-4 py-2  text-center">{formatDateTime(entry.endTime)}</td>
                                </tr>
                            ))
                        ) : (
                            <tr>
                                <td className="border px-4 py-2 text-center" colSpan={6}>
                                    No entries available
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>

            <div className="flex justify-start mb-4" style={{ margin: '2rem 0rem 2rem 0rem' }}>
                <NavigateButton
                    route="/"
                    label="Back"
                    className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
                />
            </div>
            </div>
        </>
    );    
}
