import axios from "axios";
import { get_api } from "./Methods";
export function getCategories() {
    return get_api(`https:localhost:44316/api/categories?PageNumber=10&PageSize=1`);
}