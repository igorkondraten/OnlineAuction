import { UserAddress } from './user-address.model';
import { Bid } from './bid.model';
import { Lot } from './lot.model';

export interface User {
    userProfileId: number;
    name: string;
    role: string;
    registrationDate: Date;
    email: string;
    address?: UserAddress;
    bids: Bid[];
    lots: Lot[];
}
