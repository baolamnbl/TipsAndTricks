import axios from "axios";

export async function getPosts(keyword = '', pageSize = 10, pageNumber = 1, sortColumn = '', sortOrder = '') {
    try {
        const response = await
            axios.get(`https://localhost:7085/api/posts?keyword=${keyword}&pageSize=${pageSize}&pageNumber=${pageNumber}&SortColum=${sortColumn}&SortOrder=${sortOrder}`);
        const data = response.data;
        if (data.isSucess)
            return data.result;
        else
            return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}