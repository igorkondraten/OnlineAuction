import { UserAddress } from './user-address.model';

export interface User {
    userProfileId: number;
    name: string;
    role: string;
    registerDate?: Date;
    email: string;
    address?: UserAddress;
}
