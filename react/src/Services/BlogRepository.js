import axios from "axios";

export async function getPosts(keyword = '', pageSize = 10, pageNumber = 1, sortColumn = '', sortOrder = '') {
    try {
        const response = await
            //https://localhost:44316/api/posts/?PageSize=10&PageNumber=1
            axios.get(`https://localhost:44316/api/posts?keyword=${keyword}&pageSize=${pageSize}&pageNumber=${pageNumber}&SortColum=${sortColumn}&SortOrder=${sortOrder}`);
        //axios.get(`https://localhost:44316/api/posts/?Name=${keyword}&PageSize=10&PageNumber=1`);
        const data = response.data;
        console.log(data)
        if (data.isSuccess)
            return data.result;
        else
            return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}