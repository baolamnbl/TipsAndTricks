import React, { useEffect, useState } from "react";
import PostItem from "../Components/PostItem";
import { getPosts } from "../Services/BlogRepository";
import Pager from "../Components/Pager";
import { useLocation } from "react-router-dom";
const Index = () => {
    const [postList, setPostList] = useState([]);
    const [metadata, setMetadata] = useState([]);

    function useQuery() {
        const { search } = useLocation();
        return React.useMemo(() => new URLSearchParams(search), [search]);
    }

    let query = useQuery(),
        k = query.get('k') ?? '',
        p = query.get('p') ?? 1,
        ps = query.get('ps') ?? 10;

    useEffect(() => {
        document.title = "Trang chủ";
        getPosts().then((data) => {
            console.log(data)
            if (data) {
                console.log(data)
                setPostList(data.items);
                setMetadata(data.metadata);
            } else
                setPostList([]);
        })
    }, [k, p, ps]);

    if (postList.length > 0) {
        return (
            <div className="p-4">
                {postList.map((item, index) => {
                    return (
                        <PostItem postItem={item} key={index} />
                    );
                })}
                <Pager postQuery={{ 'keywork': k }} metadata={metadata} />

            </div>
        );
    } else {
        return (
            <>
                <p>Không có bài viết nào!</p>
            </>
        );
    }
};

export default Index;