
create table emp (
    empno varchar(10) primary key,
    empname varchar(100),
    salary float,
    deptname varchar(50),
    bossno varchar(10),
    foreign key (bossno) references emp(empno)
);

create table department (
    deptname varchar(50) primary key,
    floor int,
    phone varchar(20),
    empno varchar(10) not null,
    foreign key (empno) references emp(empno)
);

create table item (
    itemname varchar(100) primary key,
    itemtype char(1),
    itemcolor varchar(50)
);

create table productsales (
    salesno varchar(10) primary key,
    saleqty int,
    itemname varchar(100),
    deptname varchar(50),
    foreign key (itemname) references item(itemname),
    foreign key (deptname) references department(deptname)
);


insert into emp (empno, empname, salary, deptname, bossno) values ('1', 'alice', 75000, null, null);
insert into emp (empno, empname, salary, deptname, bossno) values ('2', 'ned', 45000, null, '1');
insert into emp (empno, empname, salary, deptname, bossno) values ('3', 'andrew', 25000, null, '2');
insert into emp (empno, empname, salary, deptname, bossno) values ('4', 'clare', 22000, null, '2');
insert into emp (empno, empname, salary, deptname, bossno) values ('5', 'todd', 38000, null, '1');
insert into emp (empno, empname, salary, deptname, bossno) values ('6', 'nancy', 22000, null, '5');
insert into emp (empno, empname, salary, deptname, bossno) values ('7', 'brier', 43000, null, '1');
insert into emp (empno, empname, salary, deptname, bossno) values ('8', 'sarah', 56000, null, '7');
insert into emp (empno, empname, salary, deptname, bossno) values ('9', 'sophile', 35000, null, '1');
insert into emp (empno, empname, salary, deptname, bossno) values ('10', 'sanjay', 15000, null, '3');
insert into emp (empno, empname, salary, deptname, bossno) values ('11', 'rita', 15000, null, '4');
insert into emp (empno, empname, salary, deptname, bossno) values ('12', 'gigi', 16000, null, '4');
insert into emp (empno, empname, salary, deptname, bossno) values ('13', 'maggie', 11000, null, '4');
insert into emp (empno, empname, salary, deptname, bossno) values ('14', 'paul', 15000, null, '3');
insert into emp (empno, empname, salary, deptname, bossno) values ('15', 'james', 15000, null, '3');
insert into emp (empno, empname, salary, deptname, bossno) values ('16', 'pat', 15000, null, '3');
insert into emp (empno, empname, salary, deptname, bossno) values ('17', 'mark', 15000, null, '3');


insert into department (deptname, floor, phone, empno) values ('management', 5, '34', '1');
insert into department (deptname, floor, phone, empno) values ('books', 1, '81', '4');
insert into department (deptname, floor, phone, empno) values ('clothes', 2, '24', '4');
insert into department (deptname, floor, phone, empno) values ('equipment', 3, '57', '3');
insert into department (deptname, floor, phone, empno) values ('furniture', 4, '14', '3');
insert into department (deptname, floor, phone, empno) values ('navigation', 1, '41', '3');
insert into department (deptname, floor, phone, empno) values ('recreation', 2, '29', '4');
insert into department (deptname, floor, phone, empno) values ('accounting', 5, '35', '5');
insert into department (deptname, floor, phone, empno) values ('purchasing', 5, '36', '7');
insert into department (deptname, floor, phone, empno) values ('personnel', 5, '37', '9');
insert into department (deptname, floor, phone, empno) values ('marketing', 5, '38', '2');

update emp set deptname = 'management' where empno = '1';
update emp set deptname = 'marketing' where empno in ('2', '3', '4');
update emp set deptname = 'accounting' where empno in ('5', '6');
update emp set deptname = 'purchasing' where empno in ('7', '8');
update emp set deptname = 'personnel' where empno = '9';
update emp set deptname = 'navigation' where empno = '10';
update emp set deptname = 'books' where empno = '11';
update emp set deptname = 'clothes' where empno in ('12', '13');
update emp set deptname = 'equipment' where empno in ('14', '15');
update emp set deptname = 'furniture' where empno = '16';
update emp set deptname = 'recreation' where empno = '17';

alter table emp add constraint fk_emp_dept foreign key (deptname) references department(deptname);

insert into item (itemname, itemtype, itemcolor) values ('pocket knife-nile', 'e', 'brown');
insert into item (itemname, itemtype, itemcolor) values ('pocket knife-avon', 'e', 'brown');
insert into item (itemname, itemtype, itemcolor) values ('compass', 'n', null);
insert into item (itemname, itemtype, itemcolor) values ('geo positioning system', 'n', null);
insert into item (itemname, itemtype, itemcolor) values ('elephant polo stick', 'r', 'bamboo');
insert into item (itemname, itemtype, itemcolor) values ('camel saddle', 'r', 'brown');
insert into item (itemname, itemtype, itemcolor) values ('sextant', 'n', null);
insert into item (itemname, itemtype, itemcolor) values ('map measure', 'n', null);
insert into item (itemname, itemtype, itemcolor) values ('boots-snake proof', 'c', 'green');
insert into item (itemname, itemtype, itemcolor) values ('pith helmet', 'c', 'khaki');
insert into item (itemname, itemtype, itemcolor) values ('hat-polar explorer', 'c', 'white');
insert into item (itemname, itemtype, itemcolor) values ('exploring in 10 easy lessons', 'b', null);
insert into item (itemname, itemtype, itemcolor) values ('hammock', 'f', 'khaki');
insert into item (itemname, itemtype, itemcolor) values ('how to win foreign friends', 'b', null);
insert into item (itemname, itemtype, itemcolor) values ('map case', 'e', 'brown');
insert into item (itemname, itemtype, itemcolor) values ('safari chair', 'f', 'khaki');
insert into item (itemname, itemtype, itemcolor) values ('safari cooking kit', 'f', 'khaki');
insert into item (itemname, itemtype, itemcolor) values ('stetson', 'c', 'black');
insert into item (itemname, itemtype, itemcolor) values ('tent - 2 person', 'f', 'khaki');
insert into item (itemname, itemtype, itemcolor) values ('tent -8 person', 'f', 'khaki');



insert into productsales (salesno, saleqty, itemname, deptname) values ('101', 2, 'boots-snake proof', 'clothes');
insert into productsales (salesno, saleqty, itemname, deptname) values ('102', 1, 'pith helmet', 'clothes');
insert into productsales (salesno, saleqty, itemname, deptname) values ('103', 1, 'sextant', 'navigation');
insert into productsales (salesno, saleqty, itemname, deptname) values ('104', 3, 'hat-polar explorer', 'clothes');
insert into productsales (salesno, saleqty, itemname, deptname) values ('105', 5, 'pith helmet', 'equipment');
insert into productsales (salesno, saleqty, itemname, deptname) values ('106', 2, 'pocket knife-nile', 'clothes');
insert into productsales (salesno, saleqty, itemname, deptname) values ('107', 3, 'pocket knife-nile', 'recreation');
insert into productsales (salesno, saleqty, itemname, deptname) values ('108', 1, 'compass', 'navigation');
insert into productsales (salesno, saleqty, itemname, deptname) values ('109', 2, 'geo positioning system', 'navigation');
insert into productsales (salesno, saleqty, itemname, deptname) values ('110', 5, 'map measure', 'navigation');
insert into productsales (salesno, saleqty, itemname, deptname) values ('111', 1, 'geo positioning system', 'books');
insert into productsales (salesno, saleqty, itemname, deptname) values ('112', 1, 'sextant', 'books');
insert into productsales (salesno, saleqty, itemname, deptname) values ('113', 3, 'pocket knife-nile', 'books');
insert into productsales (salesno, saleqty, itemname, deptname) values ('114', 1, 'pocket knife-nile', 'navigation');
insert into productsales (salesno, saleqty, itemname, deptname) values ('115', 1, 'pocket knife-nile', 'equipment');
insert into productsales (salesno, saleqty, itemname, deptname) values ('116', 1, 'sextant', 'clothes');
insert into productsales (salesno, saleqty, itemname, deptname) values ('117', 1, 'sextant', 'equipment');
insert into productsales (salesno, saleqty, itemname, deptname) values ('118', 1, 'sextant', 'recreation');
insert into productsales (salesno, saleqty, itemname, deptname) values ('119', 1, 'sextant', 'furniture');
insert into productsales (salesno, saleqty, itemname, deptname) values ('120', 1, 'pocket knife-nile', 'furniture');
insert into productsales (salesno, saleqty, itemname, deptname) values ('121', 1, 'exploring in 10 easy lessons', 'books');
insert into productsales (salesno, saleqty, itemname, deptname) values ('122', 1, 'how to win foreign friends', 'books');
insert into productsales (salesno, saleqty, itemname, deptname) values ('123', 1, 'compass', 'books');
insert into productsales (salesno, saleqty, itemname, deptname) values ('124', 1, 'pith helmet', 'books');
insert into productsales (salesno, saleqty, itemname, deptname) values ('125', 1, 'elephant polo stick', 'recreation');
insert into productsales (salesno, saleqty, itemname, deptname) values ('126', 1, 'camel saddle', 'recreation');

select * from emp;
select * from department;
select * from item;
select * from productsales;

SELECT name
FROM sys.foreign_keys
WHERE parent_object_id = OBJECT_ID('department');

alter table department drop constraint FK__departmen__empno__1332DBDC;

drop table emp;
drop table department;
drop table item;
drop table productSales;