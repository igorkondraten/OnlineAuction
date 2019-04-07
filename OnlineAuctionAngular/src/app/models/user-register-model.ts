import { UserAddress } from './user-address';

export interface UserRegisterModel {
    Name: string;
    email: string;
    password: string;
    address?: UserAddress;
}