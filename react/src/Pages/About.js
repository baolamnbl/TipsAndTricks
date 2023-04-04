import React, { useEffect } from "react";

const Index = () => {
    useEffect(() => {
        document.title = 'Giới thiệu';
    }, []);
    return (
        <h1>
            Đây là giới thiệu
        </h1>
    );
}

export default Index;