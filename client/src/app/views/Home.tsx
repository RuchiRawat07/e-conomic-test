import * as React from "react";
import Projects from "./Projects";
import "../style.css"

export default function Home() {
    return (
        <>
           <header className="bg-gray-900 text-white flex items-center h-12 w-full">
                <div className="container mx-auto flex justify-center">
                    <a className="navbar-brand text-2xl font-poppins" href="/">
                        Timelogger
                    </a>
                </div>
            </header>

            <main>
                <div className="container mx-auto">
                    <Projects />
                </div>
            </main>
        </>
    );
}
