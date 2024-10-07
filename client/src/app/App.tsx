import * as React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Home from "./views/Home";
import AddEntry from "./views/AddEntry";
import ActivityOverview from "./views/EntryOverview";


export default function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/add-entry" element={<AddEntry />} />
                <Route path="/show-activities/:projectId" element={<ActivityOverview />} />
            </Routes>
        </Router>
    );
}
