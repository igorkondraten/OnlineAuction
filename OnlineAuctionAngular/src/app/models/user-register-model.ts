import { UserAddress } from './user-address.model';

export interface UserRegisterModel {
    name: string;
    email: string;
    password: string;
    address?: UserAddress;
}