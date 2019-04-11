import { Lot } from './lot.model';

export interface LotList {
    lots: Lot[];
    totalCount: number;
}