import { Spinner } from "react-bootstrap/";
import { Button } from "react-bootstrap/";

const Loading = () => {
    return (
        <div className="text-center">
            <Button variant='outline-success' disable style={{ border: 'none' }}>
                <Spinner
                    as='span'
                    animation='grow'
                    size='sm'
                    role='status'
                    aria-hidden='true'
                />
                &nbsp; Đang tải...

            </Button>
        </div>
    );
}

export default Loading;