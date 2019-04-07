import { UserAddress } from './user-address';

export interface User {
    id: number;
    name: string;
    role: string;
    registerDate?: Date;
    email: string;
    address?: UserAddress;
}
