import axios from "axios";

export async function getCategories() {
    try {
        const response = await
            axios.get('https:localhost:44316/api/categories?PageNumber=10&PageSize=1');

        const data = response.data;
        if (data.isSuccess)
            return data.result;
        else return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}