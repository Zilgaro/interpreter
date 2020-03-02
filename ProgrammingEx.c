#include <unistd.h>

#define SA struct sockaddr

int main(int argc, char * argv[]) {
    int sockfd, connfd, s;
    struct sockaddr_in servaddr, cli;
    struct addrinfo addr;

    if (argc != 3) {
        printf("Usage <host-address> <service-name>");
        exit(EXIT_FAILURE);
    }

    s = getaddrinfo(argv[1], argv[2], NULL, addr);

    if (s != 0) {
        perror("address info");
        exit(EXIT_FAILURE);
    }
    sockfd = socket(AF_INET, SOCK_STREAM, 0);

    if (sockfd == -1) {
        perror("socket");
        exit(EXIT_FAILURE);
    }

    bzero(&servaddr, sizeof(servaddr));
}