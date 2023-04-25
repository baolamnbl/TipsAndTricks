import axios from "axios";
import { get_api, post_api } from "./Methods";

export function getPosts(keyword = '', pageSize = 10, pageNumber = 1, sortColumn = '', sortOrder = '') {
    return get_api(`https://localhost:44316/api/posts?keyword=${keyword}&pageSize=${pageSize}&pageNumber=${pageNumber}&SortColum=${sortColumn}&SortOrder=${sortOrder}`);
}
export function getAuthors(name = '', pageSize = 10, pageNumber = 1, sortColumn = '', sortOrder = '') {
    return get_api(`https://localhost:44316/api/authors?name=${name}&pageSize=${pageSize}&pageNumber=${pageNumber}&SortColum=${sortColumn}&SortOrder=${sortOrder}`);
}
export function getFilter() {
    return get_api(`https://localhost:44316/api/posts/get-filter`);
}

export function getPostsFilter(keyword = '', authorId = '', categoryId = '', year = '', month = '', pageSize = 10, pageNumber = 1, sortColumn = '', sortOrder = '') {
    let url = new URL('https://localhost:44316/api/posts/get-posts-filter');
    keyword !== '' && url.searchParams.append('Keyword', keyword);
    authorId !== '' && url.searchParams.append('AuthorId', authorId);
    categoryId !== '' && url.searchParams.append('CategoryId', categoryId);
    year !== '' && url.searchParams.append('Year', year);
    month !== '' && url.searchParams.append('Month', month);
    sortColumn !== '' && url.searchParams.append('SortColumn', sortColumn);
    sortOrder !== '' && url.searchParams.append('SortOrder', sortOrder);
    url.searchParams.append('PageSize', pageSize);
    url.searchParams.append('PageNumber', pageNumber);
    return get_api(url.href);

}
export async function getPostById(id = 0) {
    if (id > 0)
        return get_api(`https://localhost:44316/api/posts/${id}`);
    return null
}

export function addOrUpdatePost(formData) {
    return post_api('https://localhost:44316/api/posts', formData);
}