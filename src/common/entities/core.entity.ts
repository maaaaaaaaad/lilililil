import {
  BaseEntity,
  CreateDateColumn,
  DeleteDateColumn,
  Entity,
  PrimaryGeneratedColumn,
  UpdateDateColumn,
} from 'typeorm'
import { ApiProperty } from '@nestjs/swagger'

@Entity({ name: 'CORE' })
export class CoreEntity extends BaseEntity {
  @PrimaryGeneratedColumn({ name: 'PK' })
  @ApiProperty({
    name: 'pk',
    description: 'primary key',
    type: Number,
  })
  pk: number

  @CreateDateColumn({ name: 'CREATE_AT' })
  createAt: Date

  @UpdateDateColumn({ name: 'UPDATE_AT' })
  updateAt: Date

  @DeleteDateColumn({ name: 'DELETE_AT' })
  deleteAt: Date
}
