#include <iostream>
#include <thread>
#include <chrono>

bool isOK = false;

void mainT()
{
	while (true)
	{
		//�T�u�𓮍삳����
		isOK = true;

		//���[�f�B���O��15�т傤������Ƃ���
		std::this_thread::sleep_until(std::chrono::system_clock::now() + std::chrono::seconds(15));
		
		//���[�f�B���O���I��莟��\��
		std::cout << "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" << std::endl;

		//�T�u�𖳌���
		isOK = false;

		//�I���
		return;
	}
}

void subT()
{
	while (isOK)
	{
		std::this_thread::sleep_until(std::chrono::system_clock::now() + std::chrono::seconds(1));
		std::cout << "a" << std::endl;
	}
}

int main()
{
	std::thread main(mainT);
	std::thread sub(subT);
	main.join();
	sub.join();


	return 0;
}