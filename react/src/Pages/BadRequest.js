import { Link } from 'react-router-dom';
import { useQuery } from '../Utils/Utils';

const BadRequest = () => {
    let query = useQuery(),
        redirectTo = query.get('redirecTo') ?? '/';

    return (
        <>
            ...
        </>
    );
}
export default BadRequest;